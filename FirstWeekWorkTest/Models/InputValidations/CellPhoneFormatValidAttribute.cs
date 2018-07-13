using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstWeekWorkTest.Models.InputValidations
{
    public class CellPhoneFormatValidAttribute:DataTypeAttribute
    {
        public CellPhoneFormatValidAttribute():base(DataType.Text)
        {
            ErrorMessage = "請輸入正確的電話號碼格式(ex:09xx-xxxxxx)";
        }

        // Over Ride Valid
        public override bool IsValid(object value)
        {
            // object to value->convert type.
            string str = (string)value;
            return isFormatCorrect(str);
        }

        public bool isFormatCorrect(string arg_email)
        {
            //"^(/(/d{3,4}-)|/d{3.4}-)?/d{7,8}$"
            //string pattern = @"^(/(/d{3,4}-)|/d{3.4}-)?/d{7,8}$";
         

            return true;
        }

    }
}