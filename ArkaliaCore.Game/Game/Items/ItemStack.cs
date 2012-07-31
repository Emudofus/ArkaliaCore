using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Items
{
    public class ItemStack
    {
        public Database.Models.WorldItemModel WItem { get; set; }

        public ItemStack(Database.Models.WorldItemModel witem)
        {
            this.WItem = witem;
        }

        public ItemStack Get(int quantity)
        {
            if (quantity < this.WItem.Quantity)
            {
                //return new
            }
            else if (quantity == this.WItem.Quantity)
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Delete a specified quantity from the stack
        /// </summary>
        /// <param name="quantity">Quantity to delete</param>
        /// <returns>If the stack is deleted</returns>
        public bool Delete(int quantity)
        {
            if (quantity >= this.WItem.Quantity)
            {
                this.WItem.Quantity = 0;
                return true;
            }
            else
            {
                this.WItem.Quantity -= quantity;
                return false;
            }
        }

        public bool IsSame(int templateID, string effects)
        {
            return this.WItem.TemplateID == templateID && this.WItem.Effects == effects;
        }

        public void Save()
        {
            Database.Tables.WorldItemTable.Save(this.WItem);
        }

        public int GetStackPods()
        {
            return this.WItem.Template.Pods * this.WItem.Quantity;
        }

        public ItemStack Extract(int quantity)
        {
            if (quantity <= this.WItem.Quantity)//Anti-Cheat checking
            {
                //Build new items from this stack
                var newItem = new Database.Models.WorldItemModel();
                newItem.ID = Database.Tables.WorldItemTable.TempID;
                newItem.Owner = this.WItem.Owner;
                newItem.Position = this.WItem.Position;
                newItem.TemplateID = this.WItem.TemplateID;
                newItem.Effects = this.WItem.Effects;
                newItem.Quantity = quantity;
                newItem.Engine = new Engines.EffectEngine(newItem.Effects);
                newItem.Engine.Load();

                //Build new stack
                var newStack = new ItemStack(newItem);

                //Remove the quantity duplicated
                this.WItem.Quantity -= quantity;

                return newStack;
            }
            else
            {
                return this;
            }
        }

        public override string ToString()
        {
            var pattern = new StringBuilder();
            pattern.Append(this.WItem.ID.ToString("x"))
                .Append("~")
                .Append(this.WItem.TemplateID.ToString("x"))
                .Append("~")
                .Append(this.WItem.Quantity.ToString("x"))
                .Append("~")
                .Append(this.WItem.Position == -1 ? "" : this.WItem.Position.ToString("x"))
                .Append("~")
                .Append(this.WItem.Engine.ToString());
            return pattern.ToString();
        }
    }
}
