using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SelfDC.Models
{
    class OrderItem
    {
        public int orderID { get; set; }
        public string productCode { get; set; }
        public string barcode { get; set; }
        public int qta { get; set; }

        public OrderItem(int orderID, string productCode, string barcode, int qta)
        {
            this.orderID = orderID;
            this.productCode = productCode;
            this.barcode = barcode;
            this.qta = qta;
        }

        public OrderItem(int orderID, string productCode, int qta)
        {
            this.orderID = orderID;
            this.productCode = productCode;
            this.barcode = "";
            this.qta = qta;
        }

        public OrderItem(int orderID, string productCode)
        {
            this.orderID = orderID;
            this.productCode = productCode;
            this.barcode = "";
            this.qta = 1;
        }

        public string ToString()
        {
            return string.Format("{0} | {1,7} | {2,13} | {3,3}", this.orderID, this.productCode, this.barcode, this.qta);
        }
    }
}
