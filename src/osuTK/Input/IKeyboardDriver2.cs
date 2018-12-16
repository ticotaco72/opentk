﻿using System.Collections.Generic;

namespace osuTK.Input
{
    public interface IKeyboardDriver2
    {
        /// <summary>
        /// Retrieves the combined <see cref="osuTK.Input.KeyboardState"/> for all keyboard devices.
        /// </summary>
        /// <returns>An <see cref="osuTK.Input.KeyboardState"/> structure containing the combined state for all keyboard devices.</returns>
        KeyboardState GetState();

        /// <summary>
        /// Retrieves the <see cref="osuTK.Input.KeyboardState"/> for the specified keyboard device.
        /// </summary>
        /// <param name="index">The index of the keyboard device.</param>
        /// <returns>An <see cref="osuTK.Input.KeyboardState"/> structure containing the state of the keyboard device.</returns>
        KeyboardState GetState(int index);

        /// <summary>
        /// Retrives <see cref="osuTK.Input.KeyboardState"/> for all keyboard devices.
        /// </summary>
        /// <param name="result">All states of the keyboard devices are populated into this list.</param>
        void GetStates(List<KeyboardState> result);

        /// <summary>
        /// Retrieves the device name for the keyboard device.
        /// </summary>
        /// <param name="index">The index of the keyboard device.</param>
        /// <returns>A <see cref="System.String"/> with the name of the specified device or <see cref="System.String.Empty"/>.</returns>
        /// <remarks>
        /// <para>If no device exists at the specified index, the return value is <see cref="System.String.Empty"/>.</para></remarks>
        string GetDeviceName(int index);
    }
}
