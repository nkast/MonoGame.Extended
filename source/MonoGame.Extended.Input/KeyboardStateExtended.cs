using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Extended.Input
{
    public struct KeyboardStateExtended
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public KeyboardStateExtended(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            _currentKeyboardState = currentKeyboardState;
            _previousKeyboardState = previousKeyboardState;
        }

        public bool CapsLock
        {
#if FNA
            get { return _currentKeyboardState.IsKeyDown(Keys.CapsLock); }
#else
            get { return _currentKeyboardState.CapsLock; }
#endif
        }
        public bool NumLock
        {
#if FNA
            get { return _currentKeyboardState.IsKeyDown(Keys.NumLock); }
#else
            get { return _currentKeyboardState.CapsLock; }
#endif
        }
        public bool IsShiftDown() => _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
        public bool IsControlDown() => _currentKeyboardState.IsKeyDown(Keys.LeftControl) || _currentKeyboardState.IsKeyDown(Keys.RightControl);
        public bool IsAltDown() => _currentKeyboardState.IsKeyDown(Keys.LeftAlt) || _currentKeyboardState.IsKeyDown(Keys.RightAlt);
        public bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public Keys[] GetPressedKeys() => _currentKeyboardState.GetPressedKeys();
        public void GetPressedKeys(Keys[] keys)
        {
#if FNA
            var pressedKeys = _currentKeyboardState.GetPressedKeys();
            Array.Copy(pressedKeys, keys, pressedKeys.Length);
#else
            return _currentKeyboardState.GetPressedKeys(keys);
#endif
        }
      
        /// <summary>
        /// Gets whether the given key was down on the previous state, but is now up.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>true if the key was released this state-change, otherwise false.</returns>
        [Obsolete($"Deprecated in favor of {nameof(IsKeyReleased)}")]
        public bool WasKeyJustDown(Keys key) => _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Gets whether the given key was up on the previous state, but is now down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>true if the key was pressed this state-change, otherwise false.</returns>
        [Obsolete($"Deprecated in favor of {nameof(IsKeyPressed)}")]
        public bool WasKeyJustUp(Keys key) => _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);

        /// <summary>
        /// Gets whether the given key was down on the previous state, but is now up.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>true if the key was released this state-change, otherwise false.</returns>
        public readonly bool IsKeyReleased(Keys key) => _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Gets whether the given key was up on the previous state, but is now down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>true if the key was pressed this state-change, otherwise false.</returns>
        public readonly bool IsKeyPressed(Keys key) => _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);

        public bool WasAnyKeyJustDown()
        {
#if FNA
            return _previousKeyboardState.GetPressedKeys().Length > 0;
#else
            return _previousKeyboardState.GetPressedKeyCount() > 0;
#endif
        }
   }
}
