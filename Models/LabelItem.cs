﻿
namespace SelfDC.Models
{
    public class LabelItem
    {
        public string productCode { get; set; }
        public string barcode { get; set; }
        public int qta { get; set; }

        public LabelItem(string productCode, string barcode, int qta)
        {
            this.productCode = productCode;
            this.barcode = barcode;
            this.qta = qta;
        }

        public LabelItem(string productCode, int qta)
        {
            this.productCode = productCode;
            this.barcode = "";
            this.qta = qta;
        }

        public LabelItem(string productCode)
        {
            this.productCode = productCode;
            this.barcode = "";
            this.qta = 1;
        }

        override public string ToString()
        {
            return string.Format("{0,7} | {1,13} | {2,3}", this.productCode, this.barcode, this.qta);
        }
    }
}
