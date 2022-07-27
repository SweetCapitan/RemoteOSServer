﻿using RemoteOS.OpenComputers.Data;
using RemoteOS.OpenComputers.Exceptions;

namespace RemoteOS.OpenComputers.Components
{
    [Component("inventory_controller")]
    public class InventoryContollerComponent : Component
    {
        public InventoryContollerComponent(Machine parent, Guid address) : base(parent, address)
        {
        }

        /// <param name="side">The side to analyze</param>
        /// <returns>The number of slots in the inventory on the specified side of the device.</returns>
        /// <exception cref="InvalidSideException">This side is not supported</exception>
        public async Task<int> GetInventorySize(Sides side)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            return (await Invoke("getInventorySize", side))[0];
        }
        /// <param name="side">The side that has the container</param>
        /// <param name="slot">The slot to analyze</param>
        /// <returns>A description of the stack in the inventory on the specified side of the device.</returns>
        /// <exception cref="InvalidSideException">This side is not supported</exception>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<ItemStackInfo> GetStackInSlot(Sides side, int slot)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            var res = (await Invoke("getStackInSlot", side, slot))[0];
            return new(res);
        }
        /// <param name="slot">Which slot to get</param>
        /// <returns>A description of the stack in the specified slot.</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<ItemStackInfo> GetStackInInternalSlot(int slot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            var res = (await Invoke("getStackInInternalSlot", slot))[0];
            return new(res);
        }
        /// <param name="slot">Which slot to get</param>
        /// <returns>A description of the stack in the selected slot.</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<ItemStackInfo> GetStackInInternalSlot()
        {
            var res = (await Invoke("getStackInInternalSlot"))[0];
            return new(res);
        }
        /// <inheritdoc cref="DropIntoSlot(Sides, int, int, Sides)"/>
        public async Task<(bool Success, string Reason)> DropIntoSlot(Sides side, int slot, int count = 64) => await DropIntoSlot(side, slot, count, side);
        /// <summary>
        /// Drops the selected item stack into the specified slot of an inventory.
        /// </summary>
        /// <param name="face">The face to drop items into</param>
        /// <param name="slot">The slot to drop items into</param>
        /// <param name="count">How many items to drop</param>
        /// <param name="side">Which side has the destination inventory</param>
        /// <returns>Whether the items were dropped successfully, and if they were not the reason why</returns>
        /// <exception cref="InvalidSideException">Cannot drop from that side</exception>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<(bool Success, string Reason)> DropIntoSlot(Sides face, int slot, int count, Sides side)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            var res = await Invoke("dropIntoSlot", face, slot, count, side);
            return (res[0], res[1]);
        }
        /// <inheritdoc cref="SuckFromSlot(Sides, int, int, Sides)"/>
        public async Task<(bool Success, string Reason)> SuckFromSlot(Sides side, int slot, int count = 64) => await SuckFromSlot(side, slot, count, side);
        /// <summary>
        /// Sucks items from the specified slot of an inventory.
        /// </summary>
        /// <param name="face">The face to suck from</param>
        /// <param name="slot">The slot to suck from</param>
        /// <param name="count">How many items to suck</param>
        /// <param name="side">The side that has the source container</param>
        /// <returns>Whether the items were sucked successfully, and if they were not the reason why</returns>
        /// <exception cref="InvalidSideException">Cannot suck from that side</exception>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<(bool Success, string Reason)> SuckFromSlot(Sides face, int slot, int count, Sides side)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            var res = await Invoke("suckFromSlot", face, slot, count, side);
            return (res[0], res[1]);
        }
        /// <summary>
        /// Swaps the equipped tool with the content of the currently selected inventory slot.
        /// </summary>
        /// <returns>true if items were swapped</returns>
        public async Task<bool> Equip() => (await Invoke("equip"))[0];
        /// <summary>
        /// Store an item stack description in the specified slot of the database
        /// </summary>
        /// <param name="side">The side that has the container</param>
        /// <param name="slot">The slot to store</param>
        /// <param name="database">Destination database</param>
        /// <param name="dbSlot">Database slot</param>
        /// <returns>true if the item was stores successfully</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> Store(Sides side, int slot, DatabaseComponent database, int dbSlot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("store", side, slot, database, dbSlot))[0];
        }
        /// <summary>
        /// Store an item stack description in the specified slot of the database
        /// </summary>
        /// <param name="slot">Whick slot to store</param>
        /// <param name="database">Destination database</param>
        /// <param name="dbSlot">Database slot</param>
        /// <returns>true if item was stored in database</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> StoreInternal(int slot, DatabaseComponent database, int dbSlot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("storeInternal", slot, database, dbSlot))[0];
        }
        /// <summary>
        /// Compare an item in the specified slot with one in the database with the specified address.
        /// </summary>
        /// <param name="slot">Which slot to compare</param>
        /// <param name="database">Database containing the item</param>
        /// <param name="dbSlot">Database slot</param>
        /// <param name="checkNBT">Check the nbt data</param>
        /// <returns>true if the items are the same</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> CompareToDatabase(int slot, DatabaseComponent database, int dbSlot, bool checkNBT = false)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("compareToDatabase", slot, database, dbSlot, checkNBT))[0];
        }
        /// <param name="side">The side that has the inventory</param>
        /// <param name="slotA">Slot of the first item</param>
        /// <param name="slotB">Slot of the second item</param>
        /// <param name="checkNBT">Compare with NBT data</param>
        /// <returns>Whether the items in the two specified slots of the inventory on the specified side of the device are of the same type.</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> CompareStacks(Sides side, int slotA, int slotB, bool checkNBT = false)
        {
            if (slotA <= 0 || slotB <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("compareStacks", side, slotA, slotB, checkNBT))[0];
        }
        /// <param name="side">The side that has the container</param>
        /// <param name="slot">The slot to analyze</param>
        /// <returns>The maximum number of items in the specified slot of the inventory on the specified side of the device.</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<int> GetSlotMaxStackSize(Sides side, int slot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("getSlotMaxStackSize", side, slot))[0];
        }
        /// <param name="side">The side that has the container</param>
        /// <param name="slot">The slot to analyze</param>
        /// <returns>Number of items in the specified slot of the inventory on the specified side of the device.</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<int> GetSlotStackSize(Sides side, int slot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("getSlotStackSize", side, slot))[0];
        }
        /// <param name="slot">Which slot to compare</param>
        /// <returns>Whether the stack in the selected slot is equivalent to the item in the specified slot (have shared OreDictionary IDs).</returns>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> IsEquivalentTo(int slot)
        {
            if (slot <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("isEquivalentTo", slot))[0];
        }
        /// <param name="side"></param>
        /// <param name="slotA"></param>
        /// <param name="slotB"></param>
        /// <returns>Whether the items in the two specified slots of the inventory on the specified side of the device are equivalent (have shared OreDictionary IDs).</returns>
        /// <exception cref="InvalidSideException">This side is not supported</exception>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<bool> AreStacksEquivalent(Sides side, int slotA, int slotB)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            if (slotA <= 0 || slotB <= 0) throw new InventoryException(InventoryException.NO_SUCH_SLOT);
            return (await Invoke("areStacksEquivalent", side, slotA, slotB))[0];
        }
        /// <param name="side">The side to analyze</param>
        /// <returns>The the name of the inventory on the specified side of the device.</returns>
        /// <exception cref="InvalidSideException">This side is not supported</exception>
        /// <exception cref="InventoryException">This slot does not exist</exception>
        public async Task<string> GetInventoryName(Sides side)
        {
            if (side != Sides.Front || side != Sides.Top || side != Sides.Bottom) throw new InvalidSideException("Valid sides are Front, Top, Bottom");
            return (await Invoke("getInventoryName", side))[0];
        }
    }
}
