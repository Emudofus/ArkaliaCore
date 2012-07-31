using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Items
{
    public class CharacterBag : ItemBag
    {
        public Database.Models.CharacterModel Character { get; set; }

        public CharacterBag(Database.Models.CharacterModel character)
        {
            this.Character = character;
        }

        public void UnStuff(ItemStack stack)
        {
            var bagStack = this.GetStack(stack.WItem.TemplateID, stack.WItem.Effects, -1);
            if (bagStack != null)//Add the stack quantity to the bagStack and delete the temp stack
            {
                bagStack.WItem.Quantity += stack.WItem.Quantity;
                this.Stacks.Remove(stack);
                this.RemoveStack(stack);
                this.AddedNewItemInStack(bagStack);
                bagStack.Save();
            }
            else//Only move the stack to the bag
            {
                stack.WItem.Position = -1;
                this.Character.ClientLink.Send("OM" + stack.WItem.ID + "|" + stack.WItem.Position);
                stack.Save();
            }
            //TODO: Unapply effects
        }

        public void MoveStack(ItemStack stack, int position)
        {
            if (this.Character.ClientLink != null)
            {
                if (!stack.WItem.IsStuffed && position != -1)//The stuff is in the bag
                {
                    //Anti-Cheat protection
                    if (position == 16)
                    {
                        return;
                    }

                    //Check if this stuff is already stuffed
                    var alreadyStuffed = this.GetStackByTemplateIDOutOfBag(stack.WItem.TemplateID);
                    if (alreadyStuffed != null)
                    {
                        this.Character.ClientLink.Send("BN");
                        return;
                    }
                    
                    //Check level of this item
                    if (stack.WItem.Template.Level > this.Character.Level)
                    {
                        this.Character.ClientLink.Send("OAEL");
                        return;
                    }

                    //Remove the stack if a position is occuped
                    var stackOnPos = this.GetStackAtPos(position);
                    if (stackOnPos != null)
                    {
                        UnStuff(stackOnPos);
                    }

                    if (stack.WItem.Quantity > 1)//Duplicate the stack
                    {
                        var newStack = stack.Extract(1);//Do extraction on the stack
                        newStack.WItem.Position = position;
                        this.Stacks.Add(newStack);

                        this.AddedNewItemInStack(stack);
                        this.AddedNewStack(newStack);

                        //TODO:Apply stats
                    }
                    else//Only move the current stack
                    {
                        stack.WItem.Position = position;
                        this.Character.ClientLink.Send("OM" + stack.WItem.ID + "|" + stack.WItem.Position);
                        stack.Save();
                        //TODO:Apply stats
                    }
                }
                else if(stack.WItem.IsStuffed && position == -1)//The stuff is on the character
                {
                    UnStuff(stack);
                }

                //Refresh character look on the map
                this.Character.ClientLink.RefreshLook();
                this.Character.ClientLink.RefreshPods();
            }
        }

        public override void AddedNewStack(ItemStack stack)
        {
            if (Definitions.IsLoaded)
            {
                Database.Tables.WorldItemTable.Insert(stack.WItem);
                if (this.Character.ClientLink != null)
                {
                    this.Character.ClientLink.Send("OAKO" + stack.ToString());
                    this.Character.ClientLink.RefreshPods();
                }
            }
        }

        public override void AddedNewItemInStack(ItemStack stack)
        {
            if (Definitions.IsLoaded)
            {
                Database.Tables.WorldItemTable.Save(stack.WItem);
                if (this.Character.ClientLink != null)
                {
                    this.Character.ClientLink.Send("OQ" + stack.WItem.ID + "|" + stack.WItem.Quantity);
                    this.Character.ClientLink.RefreshPods();
                }
            }
        }

        public override void RemoveStack(ItemStack stack)
        {
            if (Definitions.IsLoaded)
            {
                Database.Tables.WorldItemTable.Delete(stack.WItem);
                if (this.Character.ClientLink != null)
                {
                    this.Character.ClientLink.Send("OR" + stack.WItem.ID);
                    this.Character.ClientLink.RefreshPods();
                }
            }
        }

        public override void RemoveInStack(ItemStack stack)
        {
            if (Definitions.IsLoaded)
            {
                Database.Tables.WorldItemTable.Save(stack.WItem);
                if (this.Character.ClientLink != null)
                {
                    this.Character.ClientLink.Send("OQ" + stack.WItem.ID + "|" + stack.WItem.Quantity);
                    this.Character.ClientLink.RefreshPods();
                }
            }
        }

        public string DisplayBagConnectionPattern
        {
            get
            {
                return string.Join(";", this.Stacks);
            }
        }

        public string DisplayBagOnCharacter
        {
            get
            {
                var pattern = new StringBuilder();
                pattern.Append(this.GetStackAtPos(Enums.ItemPositionsEnum.Arme) == null ?
                                "" : this.GetStackAtPos(Enums.ItemPositionsEnum.Arme).WItem.TemplateID.ToString("x")).Append(",");
                pattern.Append(this.GetStackAtPos(Enums.ItemPositionsEnum.Chapeau) == null ?
                                "" : this.GetStackAtPos(Enums.ItemPositionsEnum.Chapeau).WItem.TemplateID.ToString("x")).Append(",");
                pattern.Append(this.GetStackAtPos(Enums.ItemPositionsEnum.Cape) == null ?
                                "" : this.GetStackAtPos(Enums.ItemPositionsEnum.Cape).WItem.TemplateID.ToString("x")).Append(",");
                pattern.Append(this.GetStackAtPos(Enums.ItemPositionsEnum.Familier) == null ?
                                "" : this.GetStackAtPos(Enums.ItemPositionsEnum.Familier).WItem.TemplateID.ToString("x")).Append(",");
                pattern.Append(this.GetStackAtPos(Enums.ItemPositionsEnum.Bouclier) == null ?
                                "" : this.GetStackAtPos(Enums.ItemPositionsEnum.Bouclier).WItem.TemplateID.ToString("x")).Append(",");
                return pattern.ToString();
            }
        }
    }
}
