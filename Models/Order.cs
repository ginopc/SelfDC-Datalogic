using System;
using System.Collections.Generic;
using System.Text;

namespace SelfDC.Models
{
    public class Order
    {
        /// <summary>
        /// Lista degli elementi dell'ordine
        /// </summary>
        private List<OrderItem> Items;

        public string Customer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Add new item into order
        /// </summary>
        public void Add(OrderItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Remove item from order
        /// </summary>
        public void Remove(OrderItem item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// Update item into order
        /// </summary>
        public void Update(OrderItem item)
        {
            foreach (OrderItem currItem in this.Items)
            {
                if (currItem.productCode == item.productCode)
                {
                    currItem.barcode = item.barcode;
                    currItem.qta = item.qta;
                    return;
                }
            }
        }

        /// <summary>
        /// Export order to file
        /// </summary>
        public void ToFile(string FileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
