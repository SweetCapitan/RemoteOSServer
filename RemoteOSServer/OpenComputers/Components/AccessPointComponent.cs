﻿namespace RemoteOS.OpenComputers.Components
{
    [Component("access_point")]
    public class AccessPointComponent : Component
    {
        public AccessPointComponent(Machine parent, Guid address) : base(parent, address)
        {
        }

        /// <returns>The signal strength (range) used when relaying messages.</returns>
        public async Task<int> GetStrength() => (await Invoke("getStrength"))[0];
        /// <summary>
        /// Set the signal strength (range) used when relaying messages.
        /// </summary>
        /// <param name="strength">New strength</param>
        public async Task SetStrength(int strength) => await Invoke("setStrength", strength);
        /// <returns>Whether the access point currently acts as a repeater (resend received wireless packets wirelessly).</returns>
        public async Task<bool> IsRepeater() => (await Invoke("isRepeater"))[0];
        /// <summary>
        /// Set whether the access point should act as a repeater.
        /// </summary>
        /// <param name="repeater">Should access point act as repeater</param>
        /// <returns></returns>
        public async Task SetRepeater(bool repeater) => await Invoke("setRepeater", repeater);

#if ROS_PROPERTIES && ROS_PROPS_UNCACHED
        public int Range
        {
            get => GetStrength().Result;
            set => SetStrength(value);
        }

        public bool Repeater
        {
            get => IsRepeater().Result;
            set => SetRepeater(value);
        }
#endif
    }
}
