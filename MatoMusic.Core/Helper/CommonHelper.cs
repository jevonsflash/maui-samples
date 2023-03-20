using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Core.Helper
{
    public class CommonHelper
    {
        public static void ShowMsg(string msg)
        {

            Application.Current.MainPage.DisplayAlert("提示", msg, "好");
        }

        public static void ShowNoAuthorized()
        {
            Application.Current.MainPage.DisplayAlert("需要权限", "MatoPlayer需要您媒体库的权限，劳烦至「设置」「隐私权」「媒体与AppleMusic」 打开权限,谢谢", "好");
        }

        public static async Task<string> PromptAsync(string title, string initialValue = null, string msg = null)
        {

            return await Application.Current.MainPage.DisplayPromptAsync(title, msg, "确定", "取消", "请输入内容", initialValue: initialValue);
        }

        public static void GoUrl(object obj)
        {
            throw new NotImplementedException();
        }

        public static void BeginInvokeOnMainThread(Action action)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(action);
        }

        public static int[] GetRandomArry(int minval, int maxval)
        {

            int[] arr = new int[maxval - minval + 1];
            int i;
            //初始化数组
            for (i = 0; i <= maxval - minval; i++)
            {
                arr[i] = i + minval;
            }
            //随机数
            Random r = new Random();
            for (int j = maxval - minval; j >= 1; j--)
            {
                int address = r.Next(0, j);
                int tmp = arr[address];
                arr[address] = arr[j];
                arr[j] = tmp;
            }
            //输出
            foreach (int k in arr)
            {
                Debug.WriteLine(k + " ");
            }
            return arr;
        }



    }



}
