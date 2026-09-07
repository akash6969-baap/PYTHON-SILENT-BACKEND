using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace AimBotConquer.Guna {

    public class AnimationManager {

        public AnimationManager(bool singular = true) {
            this.list_0 = new List<double>();
            this.list_1 = new List<Point>();
            this.list_2 = new List<AnimationDirection>();
            this.list_3 = new List<object[]>();
            this.Increment = 0.03;
            this.SecondaryIncrement = 0.03;
            this.AnimationType = AnimationType.Linear;
            this.InterruptAnimation = true;
            this.Singular = singular;
            if (this.Singular) {
                this.list_0.Add(0.0);
                this.list_1.Add(new Point(0, 0));
                this.list_2.Add(AnimationDirection.In);
            }
            this.timer_0.Tick += this.timer_0_Tick;
        }

        public bool InterruptAnimation {
            [CompilerGenerated]
            get {
                return this.bool_0;
            }
            [CompilerGenerated]
            set {
                this.bool_0 = value;
            }
        }

        public double Increment {
            [CompilerGenerated]
            get {
                return this.double_0;
            }
            [CompilerGenerated]
            set {
                this.double_0 = value;
            }
        }

        public double SecondaryIncrement {
            [CompilerGenerated]
            get {
                return this.double_1;
            }
            [CompilerGenerated]
            set {
                this.double_1 = value;
            }
        }

        public AnimationType AnimationType {
            [CompilerGenerated]
            get {
                return this.animationType_0;
            }
            [CompilerGenerated]
            set {
                this.animationType_0 = value;
            }
        }

        public bool Singular {
            [CompilerGenerated]
            get {
                return this.bool_1;
            }
            [CompilerGenerated]
            set {
                this.bool_1 = value;
            }
        }

        public event AnimationManager.AnimationFinished OnAnimationFinished {
            [CompilerGenerated]
            add {
                AnimationManager.AnimationFinished animationFinished = this.animationFinished_0;
                AnimationManager.AnimationFinished animationFinished2;
                do {
                    animationFinished2 = animationFinished;
                    AnimationManager.AnimationFinished animationFinished3 = (AnimationManager.AnimationFinished)Delegate.Combine(animationFinished2, value);
                    animationFinished = Interlocked.CompareExchange<AnimationManager.AnimationFinished>(ref this.animationFinished_0, animationFinished3, animationFinished2);
                }
                while (animationFinished != animationFinished2);
            }
            [CompilerGenerated]
            remove {
                AnimationManager.AnimationFinished animationFinished = this.animationFinished_0;
                AnimationManager.AnimationFinished animationFinished2;
                do {
                    animationFinished2 = animationFinished;
                    AnimationManager.AnimationFinished animationFinished3 = (AnimationManager.AnimationFinished)Delegate.Remove(animationFinished2, value);
                    animationFinished = Interlocked.CompareExchange<AnimationManager.AnimationFinished>(ref this.animationFinished_0, animationFinished3, animationFinished2);
                }
                while (animationFinished != animationFinished2);
            }
        }

        public event AnimationManager.AnimationProgress OnAnimationProgress {
            [CompilerGenerated]
            add {
                AnimationManager.AnimationProgress animationProgress = this.animationProgress_0;
                AnimationManager.AnimationProgress animationProgress2;
                do {
                    animationProgress2 = animationProgress;
                    AnimationManager.AnimationProgress animationProgress3 = (AnimationManager.AnimationProgress)Delegate.Combine(animationProgress2, value);
                    animationProgress = Interlocked.CompareExchange<AnimationManager.AnimationProgress>(ref this.animationProgress_0, animationProgress3, animationProgress2);
                }
                while (animationProgress != animationProgress2);
            }
            [CompilerGenerated]
            remove {
                AnimationManager.AnimationProgress animationProgress = this.animationProgress_0;
                AnimationManager.AnimationProgress animationProgress2;
                do {
                    animationProgress2 = animationProgress;
                    AnimationManager.AnimationProgress animationProgress3 = (AnimationManager.AnimationProgress)Delegate.Remove(animationProgress2, value);
                    animationProgress = Interlocked.CompareExchange<AnimationManager.AnimationProgress>(ref this.animationProgress_0, animationProgress3, animationProgress2);
                }
                while (animationProgress != animationProgress2);
            }
        }

        private void timer_0_Tick(object sender, EventArgs e) {
            for (int i = 0; i < this.list_0.Count; i++) {
                this.UpdateProgress(i);
                if (!this.Singular) {
                    if (this.list_2[i] == AnimationDirection.InOutIn && this.list_0[i] == 1.0) {
                        this.list_2[i] = AnimationDirection.InOutOut;
                    } else if (this.list_2[i] == AnimationDirection.InOutRepeatingIn && this.list_0[i] == 0.0) {
                        this.list_2[i] = AnimationDirection.InOutRepeatingOut;
                    } else {
                        if (this.list_2[i] != AnimationDirection.InOutRepeatingOut || this.list_0[i] != 0.0) {
                            if (this.list_2[i] == AnimationDirection.In && this.list_0[i] == 1.0) {
                                goto IL_120;
                            }
                            if (this.list_2[i] == AnimationDirection.Out) {
                                if (this.list_0[i] == 0.0) {
                                    goto IL_120;
                                }
                            }
                            bool flag = this.list_2[i] == AnimationDirection.InOutOut && this.list_0[i] == 0.0;
                        IL_14C:
                            if (flag) {
                                this.list_0.RemoveAt(i);
                                this.list_1.RemoveAt(i);
                                this.list_2.RemoveAt(i);
                                this.list_3.RemoveAt(i);
                                goto IL_232;
                            }
                            goto IL_232;
                        IL_120:
                            flag = true;
                            goto IL_14C;
                        }
                        this.list_2[i] = AnimationDirection.InOutRepeatingIn;
                    }
                } else if (this.list_2[i] == AnimationDirection.InOutIn && this.list_0[i] == 1.0) {
                    this.list_2[i] = AnimationDirection.InOutOut;
                } else if (this.list_2[i] == AnimationDirection.InOutRepeatingIn && this.list_0[i] == 1.0) {
                    this.list_2[i] = AnimationDirection.InOutRepeatingOut;
                } else if (this.list_2[i] == AnimationDirection.InOutRepeatingOut && this.list_0[i] == 0.0) {
                    this.list_2[i] = AnimationDirection.InOutRepeatingIn;
                }
            IL_232:;
            }
            AnimationManager.AnimationProgress animationProgress = this.animationProgress_0;
            if (animationProgress != null) {
                animationProgress(this);
            }
        }

        public bool IsAnimating() {
            return this.timer_0.Enabled;
        }

        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null) {
            this.StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null) {
            if (!this.IsAnimating() || this.InterruptAnimation) {
                if (this.Singular && this.list_2.Count > 0) {
                    this.list_2[0] = animationDirection;
                } else {
                    this.list_2.Add(animationDirection);
                }
                if (this.Singular && this.list_1.Count > 0) {
                    this.list_1[0] = animationSource;
                } else {
                    this.list_1.Add(animationSource);
                }
                if (!this.Singular || this.list_0.Count <= 0) {
                    switch (this.list_2[this.list_2.Count - 1]) {
                        case AnimationDirection.In:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.InOutRepeatingIn:
                            this.list_0.Add(0.0);
                            break;
                        case AnimationDirection.Out:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.InOutRepeatingOut:
                            this.list_0.Add(1.0);
                            break;
                        default:
                            throw new Exception("Invalid AnimationDirection");
                    }
                }
                if (this.Singular && this.list_3.Count > 0) {
                    this.list_3[0] = data ?? new object[0];
                } else {
                    this.list_3.Add(data ?? new object[0]);
                }
            }
            this.timer_0.Start();
        }

        public void UpdateProgress(int index) {
            switch (this.list_2[index]) {
                case AnimationDirection.In:
                case AnimationDirection.InOutIn:
                case AnimationDirection.InOutRepeatingIn:
                    this.method_0(index);
                    break;
                case AnimationDirection.Out:
                case AnimationDirection.InOutOut:
                case AnimationDirection.InOutRepeatingOut:
                    this.method_1(index);
                    break;
                default:
                    throw new Exception("No AnimationDirection has been set");
            }
        }

        private void method_0(int int_0) {
            List<double> list = this.list_0;
            list[int_0] += this.Increment;
            if (this.list_0[int_0] > 1.0) {
                this.list_0[int_0] = 1.0;
                for (int i = 0; i < this.GetAnimationCount(); i++) {
                    if (this.list_2[i] == AnimationDirection.InOutIn || this.list_2[i] == AnimationDirection.InOutRepeatingIn || this.list_2[i] == AnimationDirection.InOutRepeatingOut || (this.list_2[i] == AnimationDirection.InOutOut && this.list_0[i] != 1.0) || (this.list_2[i] == AnimationDirection.In && this.list_0[i] != 1.0)) {
                        return;
                    }
                }
                this.timer_0.Stop();
                AnimationManager.AnimationFinished animationFinished = this.animationFinished_0;
                if (animationFinished != null) {
                    animationFinished(this);
                }
            }
        }

        private void method_1(int int_0) {
            List<double> list = this.list_0;
            list[int_0] -= ((this.list_2[int_0] == AnimationDirection.InOutOut || this.list_2[int_0] == AnimationDirection.InOutRepeatingOut) ? this.SecondaryIncrement : this.Increment);
            if (this.list_0[int_0] < 0.0) {
                this.list_0[int_0] = 0.0;
                for (int i = 0; i < this.GetAnimationCount(); i++) {
                    if (this.list_2[i] == AnimationDirection.InOutIn || this.list_2[i] == AnimationDirection.InOutRepeatingIn || this.list_2[i] == AnimationDirection.InOutRepeatingOut || (this.list_2[i] == AnimationDirection.InOutOut && this.list_0[i] != 0.0) || (this.list_2[i] == AnimationDirection.Out && this.list_0[i] != 0.0)) {
                        return;
                    }
                }
                this.timer_0.Stop();
                AnimationManager.AnimationFinished animationFinished = this.animationFinished_0;
                if (animationFinished != null) {
                    animationFinished(this);
                }
            }
        }

        public double GetProgress() {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_0.Count == 0) {
                throw new Exception("Invalid animation");
            }
            return this.GetProgress(0);
        }

        public double GetProgress(int index) {
            if (index >= this.GetAnimationCount()) {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            double num;
            switch (this.AnimationType) {
                case AnimationType.Linear:
                    num = AnimationLinear.CalculateProgress(this.list_0[index]);
                    break;
                case AnimationType.EaseInOut:
                    num = AnimationEaseInOut.CalculateProgress(this.list_0[index]);
                    break;
                case AnimationType.EaseOut:
                    num = AnimationEaseOut.CalculateProgress(this.list_0[index]);
                    break;
                case AnimationType.CustomQuadratic:
                    num = AnimationCustomQuadratic.CalculateProgress(this.list_0[index]);
                    break;
                default:
                    throw new NotImplementedException("The given AnimationType is not implemented");
            }
            return num;
        }

        public Point GetSource(int index) {
            if (index >= this.GetAnimationCount()) {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this.list_1[index];
        }

        public Point GetSource() {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_1.Count == 0) {
                throw new Exception("Invalid animation");
            }
            return this.list_1[0];
        }

        public AnimationDirection GetDirection() {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_2.Count == 0) {
                throw new Exception("Invalid animation");
            }
            return this.list_2[0];
        }

        public AnimationDirection GetDirection(int index) {
            if (index >= this.list_2.Count) {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this.list_2[index];
        }

        public object[] GetData() {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_3.Count == 0) {
                throw new Exception("Invalid animation");
            }
            return this.list_3[0];
        }

        public object[] GetData(int index) {
            if (index >= this.list_3.Count) {
                throw new IndexOutOfRangeException("Invalid animation index");
            }
            return this.list_3[index];
        }

        public int GetAnimationCount() {
            return this.list_0.Count;
        }

        public void SetProgress(double progress) {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_0.Count == 0) {
                throw new Exception("Invalid animation");
            }
            this.list_0[0] = progress;
        }

        public void SetDirection(AnimationDirection direction) {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_0.Count == 0) {
                throw new Exception("Invalid animation");
            }
            this.list_2[0] = direction;
        }

        public void SetData(object[] data) {
            if (!this.Singular) {
                throw new Exception("Animation is not set to Singular.");
            }
            if (this.list_3.Count == 0) {
                throw new Exception("Invalid animation");
            }
            this.list_3[0] = data;
        }

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool bool_0;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double double_0;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double double_1;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationType animationType_0;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool bool_1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private AnimationManager.AnimationFinished animationFinished_0;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationManager.AnimationProgress animationProgress_0;

        private readonly List<double> list_0;

        private readonly List<Point> list_1;

        private readonly List<AnimationDirection> list_2;

        private readonly List<object[]> list_3;

        private const double double_2 = 0.0;

        private const double double_3 = 1.0;

        private readonly System.Windows.Forms.Timer timer_0 = new System.Windows.Forms.Timer {
            Interval = 5,
            Enabled = false
        };

        public delegate void AnimationFinished(object sender);

        public delegate void AnimationProgress(object sender);
    }

    public enum AnimationDirection {

        In,

        Out,

        InOutIn,

        InOutOut,

        InOutRepeatingIn,

        InOutRepeatingOut
    }

    public enum AnimationType {

        Linear,

        EaseInOut,

        EaseOut,

        CustomQuadratic
    }

    public static class AnimationLinear {

        public static double CalculateProgress(double progress) {
            return progress;
        }
    }

    public static class AnimationEaseOut {

        public static double CalculateProgress(double progress) {
            return -1.0 * progress * (progress - 2.0);
        }
    }

    public static class AnimationCustomQuadratic {

        public static double CalculateProgress(double progress) {
            double num = 0.6;
            return 1.0 - Math.Cos((Math.Max(progress, num) - num) * 3.1415926535897931 / 0.8);
        }
    }

    public static class AnimationEaseInOut {

        static AnimationEaseInOut() {
        }

        public static double CalculateProgress(double progress) {
            return AnimationEaseInOut.smethod_0(progress);
        }

        private static double smethod_0(double double_1) {
            return double_1 - Math.Sin(double_1 * 2.0 * AnimationEaseInOut.double_0) / (2.0 * AnimationEaseInOut.double_0);
        }

        public static double double_0 = 3.1415926535897931;

        public static double PI_HALF = 1.5707963267948966;
    }
}