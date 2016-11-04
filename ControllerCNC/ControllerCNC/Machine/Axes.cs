﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerCNC.Machine
{
    class Axes : InstructionCNC
    {
        /// <summary>
        /// Instruction for x axis.
        /// </summary>
        internal readonly StepInstrution InstructionX;

        /// <summary>
        /// Instruction for y axis.
        /// </summary>
        internal readonly StepInstrution InstructionY;

        /// <summary>
        /// Instruction for u axis.
        /// </summary>
        internal readonly StepInstrution InstructionU;

        /// <summary>
        /// Instruction for v axis.
        /// </summary>
        internal readonly StepInstrution InstructionV;

        private Axes(StepInstrution x, StepInstrution y, StepInstrution u, StepInstrution v)
        {
            InstructionX = x;
            InstructionY = y;
            InstructionU = u;
            InstructionV = v;

            //check instruction compatibility
            Type detectedType = null;
            foreach (var instruction in new[] { InstructionX, InstructionY, InstructionU, InstructionV })
            {
                if (instruction == null)
                    continue;

                if (detectedType == null)
                    detectedType = instruction.GetType();

                if (instruction.GetType() != detectedType)
                    throw new NotSupportedException("All combined instructions must have same type.");
            }

            if (detectedType == null)
                throw new NullReferenceException("At least one axis has to be set by instruction.");
        }

        /// <summary>
        /// Combines instructions for x and y axes.
        /// </summary>
        /// <param name="x">Instruction for x.</param>
        /// <param name="y">Instruction for y.</param>
        public static Axes XY(StepInstrution x, StepInstrution y)
        {
            return new Axes(x, y, null, null);
        }

        /// <summary>
        /// Instruct x axis.
        /// </summary>
        /// <param name="x">Instruction for x.</param>
        public static Axes X(StepInstrution x)
        {
            return new Axes(x, null, null, null);
        }

        /// <summary>
        /// Instruct y axis.
        /// </summary>
        /// <param name="y">Instruction for y.</param>
        public static Axes Y(StepInstrution y)
        {
            return new Axes(null, y, null, null);
        }

        /// </inheritdoc>
        internal override byte[] GetInstructionBytes()
        {
            var payloadLength = getInstructionPayloadLength();
            var groupBytes = new byte[1 + 4 * payloadLength];

            //identifier is shared for whole group
            groupBytes[0] = getInstructionIdentifier();

            var instructionIndex = 0;
            foreach (var instruction in new[] { InstructionX, InstructionY, InstructionU, InstructionV })
            {
                var instructionOffset = 1 + instructionIndex * payloadLength;
                ++instructionIndex;

                if (instruction == null)
                    //we leave zeros as a payload
                    continue;

                var instructionBytes = instruction.GetInstructionBytes();
                for (var i = 1; i < instructionBytes.Length; ++i)
                {
                    groupBytes[instructionOffset + i - 1] = instructionBytes[i];
                }
            }

            return groupBytes;
        }

        /// <summary>
        /// Gets identifier which is used for whole instruction group.
        /// </summary>
        private byte getInstructionIdentifier()
        {
            return getNonNullInstruction().GetInstructionBytes()[0];
        }

        /// <summary>
        /// Gets length of the instruction payload (instruction without identifier).
        /// </summary>
        private int getInstructionPayloadLength()
        {
            return getNonNullInstruction().GetInstructionBytes().Length - 1;
        }

        /// <summary>
        /// Gets an instruction for axis which is not null.
        /// </summary>
        private InstructionCNC getNonNullInstruction()
        {
            foreach (var instruction in new[] { InstructionX, InstructionY, InstructionU, InstructionV })
            {
                if (instruction != null)
                    return instruction;
            }

            throw new NullReferenceException("There has to be at least one non-null instruction.");
        }
    }
}