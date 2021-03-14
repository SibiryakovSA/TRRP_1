using System;
using System.Collections.Generic;
using System.Text;
using VkNet.Utils.AntiCaptcha;

namespace TRRP_Lab1
{
    //решение капчи
    class CaptchaResolver : ICaptchaSolver
    {
        public void CaptchaIsFalse()
        {
            Console.WriteLine("Капча решена неверно");
        }

        public string Solve(string url)
        {
            Console.WriteLine("Для продолжения решите капчу:" + url);
            return Console.ReadLine();
        }

        public CaptchaResolver()
        {

        }
    }
}
