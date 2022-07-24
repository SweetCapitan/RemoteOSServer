﻿namespace RemoteOS.OpenComputers.Components
{
    [Component("piston")]
    public class PistonComponent : Component
    {
        public PistonComponent(Machine parent, Guid address) : base(parent, address)
        {
        }

        bool? _isSticky;

        /// <returns>true if the piston is sticky, i.e. it can also pull.</returns>
        public async Task<bool> IsSticky() => _isSticky ??= (await Invoke("isSticky"))[0];
        /// <summary>
        /// Tries to push the block on the specified side of the container of the upgrade.
        /// </summary>
        /// <param name="side">The side to push</param>
        /// <returns>true if block was pushed</returns>
        public async Task<bool> Push(Sides side) => (await Invoke("push", side))[0];
        /// <summary>
        /// Tries to reach out to the side given and pull a block similar to a vanilla sticky piston.
        /// </summary>
        /// <param name="side">The side to pull</param>
        /// <returns>true if block was pulled</returns>
        public async Task<bool> Pull(Sides side) => (await Invoke("pull", side))[0];

#if ROS_PROPERTIES
        public bool Sticky => IsSticky().Result;
#endif
    }
}
