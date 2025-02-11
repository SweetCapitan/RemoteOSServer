﻿using System.Drawing;
using System.Numerics;
using RemoteOS.Helpers;

namespace RemoteOS.OpenComputers.Components
{
    [Component("printer3d")]
    public partial class Printer3DComponent : Component
    {
        public Printer3DComponent(Machine parent, Guid address) : base(parent, address)
        {
        }

        int? _maxShapes;

        /// <summary>
        /// Commit and begin printing the current configuration.
        /// </summary>
        /// <param name="count">How much items to print</param>
        /// <returns>true if printing has started</returns>
        public async Task<bool> Print(int count = 1) => await InvokeFirst("commit", count);

        /// <inheritdoc cref="AddShape(int, int, int, int, int, int, string, bool, Color)"/>
        public Task AddShape(int minX, int minY, int minZ, int maxX, int maxY, int maxZ, string texture, Color tint) => AddShape(minX, minY, minZ, maxX, maxY, maxZ, texture, false, tint);
        
        /// <inheritdoc cref="AddShape(int, int, int, int, int, int, string, bool, Color)"/>
        public Task AddShape(int minX, int minY, int minZ, int maxX, int maxY, int maxZ, string texture, bool state = false) => AddShape(minX, minY, minZ, maxX, maxY, maxZ, texture, state, Color.Transparent);
        
        /// <inheritdoc cref="AddShape(int, int, int, int, int, int, string, bool, Color)"/>
        /// <param name="start">Start of the shape</param>
        /// <param name="end">End of the shape</param>
        public Task AddShape(Vector3 start, Vector3 end, string texture, bool state, Color tint) => AddShape((int)start.X, (int)start.Y, (int)start.Z, (int)end.X, (int)end.Y, (int)end.Z, texture, state, tint);
        
        /// <inheritdoc cref="AddShape(int, int, int, int, int, int, string, bool, Color)"/>
        /// /// <param name="start">Start of the shape</param>
        /// <param name="end">End of the shape</param>
        public Task AddShape(Vector3 start, Vector3 end, string texture, bool state) => AddShape((int)start.X, (int)start.Y, (int)start.Z, (int)end.X, (int)end.Y, (int)end.Z, texture, state, Color.Transparent);
        
        /// <inheritdoc cref="AddShape(int, int, int, int, int, int, string, bool, Color)"/>
        /// /// <param name="start">Start of the shape</param>
        /// <param name="end">End of the shape</param>
        public Task AddShape(Vector3 start, Vector3 end, string texture, Color tint) => AddShape((int)start.X, (int)start.Y, (int)start.Z, (int)end.X, (int)end.Y, (int)end.Z, texture, false, tint);
       
        /// <summary>
        /// Adds a shape to the printers configuration, optionally specifying whether it is for the off or on state.
        /// </summary>
        /// <param name="minX">Start X coordinate</param>
        /// <param name="minY">Start Y coordinate</param>
        /// <param name="minZ">Start Z coordinate</param>
        /// <param name="maxX">End X coordinate</param>
        /// <param name="maxY">End Y coordinate</param>
        /// <param name="maxZ">End Z coordinate</param>
        /// <param name="texture">Texture for this shape</param>
        /// <param name="state">Which state this shape belongs to</param>
        /// <param name="tint">Color tint for this shape</param>
        /// <exception cref="ArgumentException">Shape is empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Shape is outside of bounds</exception>
        public Task AddShape(int minX, int minY, int minZ, int maxX, int maxY, int maxZ, string texture, bool state, Color tint)
        {
            if (minX == maxX || minY == maxY || minZ == maxZ) throw new ArgumentException("Empty block");
            if (
                minX < 0 || minX > 15 ||
                minY < 0 || minY > 15 ||
                minZ < 0 || minZ > 15 ||
                maxX < 0 || maxX > 15 ||
                maxY < 0 || maxY > 15 ||
                maxZ < 0 || maxZ > 15
            ) throw new ArgumentOutOfRangeException("Coordinates must be in range 0..15");
            return Invoke("addShape", minX, minY, minZ, maxX, maxY, maxZ, texture, state, tint);
        }

        /// <summary>
        /// Resets the configuration of the printer and stop printing (current job will finish).
        /// </summary>
        public partial Task Reset();

        /// <returns>The current label for the block being printed.</returns>
        public partial Task<string> GetLabel();

        /// <summary>
        /// Set a label for the block being printed.
        /// </summary>
        /// <param name="label">New label</param>
        public partial Task SetLabel(string label);

        /// <returns>The current tooltip for the block being printed.</returns>
        public partial Task<string> GetTooltip();

        /// <summary>
        /// Set a tooltip for the block being printed.
        /// </summary>
        /// <param name="tooltip">New tooltip</param>
        public partial Task SetTooltip(string tooltip);

        /// <returns>Whether the printed block should automatically return to its off state.</returns>
        public partial Task<bool> IsButtonMode();

        /// <summary>
        /// Set whether the printed block should automatically return to its off state.
        /// </summary>
        /// <param name="button">Is button mode</param>
        public partial Task SetButtonMode(bool button);

        /// <returns>Whether the printed block should emit redstone when in its active state.</returns>
        public async Task<int> GetRedstoneLevel() => (await Invoke("isRedstoneEmitter"))[1];

        /// <summary>
        /// Set whether the printed block should emit redstone when in its active state.
        /// </summary>
        /// <param name="level">New redstone level</param>
        /// <exception cref="ArgumentOutOfRangeException">Invalid redstone level</exception>
        public Task SetRedstoneLevel(int level)
        {
            if (level < 0 || level > 15) throw new ArgumentOutOfRangeException(nameof(level), "Redstone level cannot be less than 0 or higher than 15");
            return Invoke("setRedstoneEmitter", level);
        }

        /// <returns>Whether the printed block should be collidable or not.</returns>
        public async Task<(bool Default, bool Active)> GetCollideMode()
        {
            var r = await Invoke("isCollidable");
            return (r[0], r[1]);
        }

        /// <summary>
        /// Set whether the printed block should be collidable or not.
        /// </summary>
        /// <param name="def">Default collide mode</param>
        /// <param name="active">Active collide mode</param>
        /// <returns></returns>
        public partial Task SetCollidable(bool def, bool active);

        /// <returns>Which light level the printed block should have.</returns>
        public partial Task<int> GetLightLevel();

        /// <summary>
        /// Set what light level the printed block should have.
        /// </summary>
        /// <param name="level">New light level</param>
        public partial Task SetLightLevel(int level);

        /// <returns>The number of shapes in the current configuration.</returns>
        public partial Task<int> GetShapeCount();

        /// <returns>The maximum allowed number of shapes.</returns>
        public async Task<int> GetMaxShapeCount() => _maxShapes ??= await InvokeFirst("getMaxShapeCount");

        /// <returns>The current state of the printer, `busy' or `idle', followed by the progress or model validity, respectively.</returns>
        public async Task<(bool CanPrint, string Status, double Progress)> GetStatus()
        {
            var res = await Invoke("status");
            var isb = res[1].IsBoolean;
            return (isb ? res[1] : false, res[0], isb ? 0 : res[1]);
        }

#if ROS_PROPERTIES
#if ROS_PROPS_UNCACHED
        public string Label
        {
            get => GetLabel().Result;
            set => SetLabel(value);
        }

        public string ToolTip
        {
            get => GetTooltip().Result;
            set => SetTooltip(value);
        }

        public bool ButtonMode
        {
            get => IsButtonMode().Result;
            set => SetButtonMode(value);
        }

        public int RedstoneLevel
        {
            get => GetRedstoneLevel().Result;
            set => SetRedstoneLevel(value);
        }

        public (bool Default, bool Active) CollideMode
        {
            get => GetCollideMode().Result;
            set => SetCollideMode(value.Default, value.Active);
        }

        public int LightLevel
        {
            get => GetLightLevel().Result;
            set => SetLightLevel(value);
        }
        public int ShapeCount => GetShapeCount().Result;

        public (bool CanPrint, string Status, double Progress) Status => GetStatus().Result;
#endif

        public int MaxShapeCount => GetMaxShapeCount().Result;
#endif
    }
}
