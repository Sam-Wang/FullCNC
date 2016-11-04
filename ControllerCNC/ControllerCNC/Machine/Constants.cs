﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ControllerCNC.Primitives;

namespace ControllerCNC.Machine
{
    /// <summary>
    /// Encapsulates constants related to the CNC machine. All the configuration
    /// like machine limits and capabilities (not communication related staff) has to be here.
    /// </summary>
    static class Constants
    {
        /// <summary>
        /// Time scale of the machine. (2MHz)
        /// </summary>
        public static readonly uint TimerFrequency = 2000000;

        /// <summary>
        /// How many steps for single revolution has to be done.
        /// </summary>
        public static readonly uint StepsPerRevolution = 400;

        /// <summary>
        /// Screw with 1.25mm per revolution.
        /// </summary>
        public static readonly double MilimetersPerStep = 1.25 / StepsPerRevolution;

        /// <summary>
        /// Maximal safe acceleration in steps/s^2.
        /// </summary>
        public static readonly uint MaxAcceleration = 200 * StepsPerRevolution;

        /// <summary>
        /// X axis is 460mm long
        /// </summary>
        public static readonly uint MaxStepsX = 460 * StepsPerRevolution * 100 / 125;

        /// <summary>
        /// Y axis is 256mm long
        /// </summary>
        public static readonly uint MaxStepsY = 256 * StepsPerRevolution * 100 / 125;

        /// <summary>
        /// DeltaT which can be safely used after stand still.
        /// </summary>
        public static readonly int StartDeltaT = 2000;

        /// <summary>
        /// Fastest DeltaT which is supported
        /// </summary>
        public static int FastestDeltaT = 350;

        /// <summary>
        /// Speed which is safe to turn around without any additional delays.
        /// </summary>
        public static readonly Speed ReverseSafeSpeed = Speed.FromDelta(StartDeltaT);

        /// <summary>
        /// Maximal speed for head moving in a plane (X,Y or U,V).
        /// </summary>
        public static readonly Speed MaxPlaneSpeed = Speed.FromDelta(FastestDeltaT);

        /// <summary>
        /// Maximal speed for head moving in a plane (X,Y or U,V).
        /// </summary>
        public static readonly Acceleration MaxPlaneAcceleration = new Acceleration(new Speed(MaxAcceleration, TimerFrequency), TimerFrequency);
    }
}