# esp.py
import pymem
import pymem.process
import pygame
import math
from offset import Offsets

# === CONFIG ===
ESP_LINE = 1
ESP_BOX = 1
ESP_HEALTH = 1
ESP_NAME = 1
ESP_DISTANCE = 1
ESP_SKELETON = 1

PROCESS_NAME = "HD-Player.exe"

# === WORLD TO SCREEN ===
def world_to_screen(matrix, pos, width, height):
    clip_coords = [
        pos[0] * matrix[0] + pos[1] * matrix[4] + pos[2] * matrix[8] + matrix[12],
        pos[0] * matrix[1] + pos[1] * matrix[5] + pos[2] * matrix[9] + matrix[13],
        pos[0] * matrix[2] + pos[1] * matrix[6] + pos[2] * matrix[10] + matrix[14],
        pos[0] * matrix[3] + pos[1] * matrix[7] + pos[2] * matrix[11] + matrix[15]
    ]
    if clip_coords[3] < 0.1:
        return None
    NDC = [clip_coords[0]/clip_coords[3], clip_coords[1]/clip_coords[3]]
    return [
        (width/2 * NDC[0]) + (NDC[0] + width/2),
        -(height/2 * NDC[1]) + (NDC[1] + height/2)
    ]

# === ESP CLASS ===
class ESP:
    def __init__(self):
        self.pm = pymem.Pymem(PROCESS_NAME)
        self.client = pymem.process.module_from_name(self.pm.process_handle, PROCESS_NAME).lpBaseOfDll

        pygame.init()
        self.width, self.height = 1280, 720
        self.screen = pygame.display.set_mode((self.width, self.height), pygame.NOFRAME)
        pygame.display.set_caption("ESP Overlay")

    def read_entity(self, base, index):
        entity = self.pm.read_int(base + index * 0x4)
        if not entity:
            return None
        try:
            health = self.pm.read_int(entity + Offsets.Health)
            name = self.pm.read_string(entity + Offsets.Name, 16)
            pos = [
                self.pm.read_float(entity + Offsets.Position),
                self.pm.read_float(entity + Offsets.Position + 4),
                self.pm.read_float(entity + Offsets.Position + 8),
            ]
            return {"entity": entity, "health": health, "name": name, "pos": pos}
        except:
            return None

    def draw_box(self, x, y, w, h, color=(0,255,0)):
        pygame.draw.rect(self.screen, color, pygame.Rect(x, y, w, h), 1)

    def draw_line(self, x1, y1, x2, y2, color=(255,0,0)):
        pygame.draw.line(self.screen, color, (x1, y1), (x2, y2), 1)

    def draw_text(self, text, x, y, color=(255,255,255)):
        font = pygame.font.SysFont("Arial", 14)
        render = font.render(text, True, color)
        self.screen.blit(render, (x, y))

    def run(self):
        running = True
        while running:
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    running = False

            self.screen.fill((0,0,0,0))  # clear overlay

            entity_list = self.pm.read_int(self.client + Offsets.EntityList)
            for i in range(32):  # example max 32 entities
                ent = self.read_entity(entity_list, i)
                if not ent or ent["health"] <= 0: continue

                # Convert world to screen
                screen_pos = [self.width//2, self.height//2]  # dummy center
                # TODO: integrate real matrix transform from game

                # ESP Line
                if ESP_LINE:
                    self.draw_line(self.width//2, self.height, screen_pos[0], screen_pos[1])

                # ESP Box
                if ESP_BOX:
                    self.draw_box(screen_pos[0]-20, screen_pos[1]-50, 40, 100)

                # ESP Health
                if ESP_HEALTH:
                    self.draw_text(f"HP:{ent['health']}", screen_pos[0], screen_pos[1]-60, (0,255,0))

                # ESP Name
                if ESP_NAME:
                    self.draw_text(ent["name"], screen_pos[0], screen_pos[1]-75, (255,255,255))

                # ESP Distance
                if ESP_DISTANCE:
                    self.draw_text("Dist:100m", screen_pos[0], screen_pos[1]-90, (0,200,255))

                # ESP Skeleton (simple line example)
                if ESP_SKELETON:
                    self.draw_line(screen_pos[0], screen_pos[1]-50, screen_pos[0], screen_pos[1]+50, (255,255,0))

            pygame.display.flip()
        pygame.quit()
