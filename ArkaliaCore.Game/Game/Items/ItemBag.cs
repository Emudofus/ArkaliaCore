using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Items
{
    public class ItemBag
    {
        public List<ItemStack> Stacks = new List<ItemStack>();

        public void Add(Database.Models.WorldItemModel item)
        {
            var stack = GetStack(item.TemplateID, item.Effects, -1);

        CreateStack://Goto state, is ugly :/

            if (stack == null)
            {
                stack = new ItemStack(item);
                AddedNewStack(stack);
                this.Stacks.Add(stack);
                //Utilities.Logger.Debug("Created new item stack");
                return;
            }

            if (stack != null && stack.WItem.Position == -1 && item.Position == -1)
            {
                stack.WItem.Quantity += item.Quantity;
                AddedNewItemInStack(stack);
                //Utilities.Logger.Debug("Modified existing item stack");
                return;
            }
            else//Its ugly but is functional
            {
                stack = null;
                goto CreateStack;
            }     
        }

        public void Remove(ItemStack item, int quantity)
        {
            if (item.Delete(quantity))
            {
                this.Stacks.Remove(item);
                this.RemoveStack(item);
            }
            else
            {
                this.RemoveInStack(item);
            }
        }

        public ItemStack GetStack(int id)
        {
            if (this.haveThisStack(id))
            {
                return this.Stacks.FirstOrDefault(x => x.WItem.ID == id);
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStack(int templateID, string effects)
        {
            if (this.haveThisStack(templateID, effects))
            {
                return this.Stacks.FirstOrDefault(x => x.IsSame(templateID, effects));
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStack(int templateID, string effects, int position)
        {
            if (this.haveThisStack(templateID, effects, position))
            {
                return this.Stacks.FirstOrDefault(x => x.IsSame(templateID, effects) && x.WItem.Position == position);
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStackByTemplateID(int id, int position = -1)
        {
            if (this.Stacks.FindAll(x => x.WItem.TemplateID == id && x.WItem.Position == position).Count > 0)
            {
                return this.Stacks.FirstOrDefault(x => x.WItem.TemplateID == id && x.WItem.Position == position);
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStackAtPos(int position)
        {
            if (this.Stacks.FindAll(x => x.WItem.Position == position).Count > 0)
            {
                return this.Stacks.FirstOrDefault(x => x.WItem.Position == position);
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStackAtPos(Enums.ItemPositionsEnum position)
        {
            if (this.Stacks.FindAll(x => x.WItem.Position == (int)position).Count > 0)
            {
                return this.Stacks.FirstOrDefault(x => x.WItem.Position == (int)position);
            }
            else
            {
                return null;
            }
        }

        public ItemStack GetStackByTemplateIDOutOfBag(int id)
        {
            if (this.Stacks.FindAll(x => x.WItem.TemplateID == id && x.WItem.Position != -1).Count > 0)
            {
                return this.Stacks.FirstOrDefault(x => x.WItem.TemplateID == id && x.WItem.Position != -1);
            }
            else
            {
                return null;
            }
        }

        public void SaveBag()
        {
            foreach (var stack in this.Stacks)
            {
                stack.Save();
            }
        }

        private bool haveThisStack(int id)
        {
            return this.Stacks.FindAll(x => x.WItem.ID == id).Count > 0;
        }

        private bool haveThisStack(int templateID, string effects)
        {
            return this.Stacks.FindAll(x => x.IsSame(templateID, effects)).Count > 0;
        }

        private bool haveThisStack(int templateID, string effects, int position)
        {
            return this.Stacks.FindAll(x => x.IsSame(templateID, effects) && x.WItem.Position == position).Count > 0;
        }

        public int BagWeight
        {
            get
            {
                var value = 0;
                foreach (var stack in this.Stacks)
                {
                    value += stack.GetStackPods();
                }
                return value;
            }
        }

        #region Need to overrides

        public virtual void AddedNewStack(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        public virtual void AddedNewItemInStack(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveStack(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveInStack(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
