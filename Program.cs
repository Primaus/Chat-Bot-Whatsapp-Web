using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace cahtbot_whatsapp_2022
{
    class Program
    {
        static void main_brain(IWebDriver driver, WebDriverWait wait)
        {
            wait.Until(drv => drv.FindElement(By.XPath("//span[contains(@aria-label, 'mensaje no leído') or " +
                "contains(@aria-label, 'mensajes no leídos')] | //div[contains(@aria-label, 'Lista de mensajes')]" +
                "//div[last()][contains(@class, 'message-in')]")));

            try
            {
                driver.FindElement(By.XPath("//span[contains(@aria-label, 'mensaje no leído') or " +
                "contains(@aria-label, 'mensajes no leídos')]//ancestor::div[@class='_3OvU8']")).Click();
            }
            catch { }
        }

        static void Main(string[] args)
        {
            Dictionary<string, int> lst_user = new Dictionary<string, int>();

            ChromeOptions options = new ChromeOptions();

            options.AddArguments("--start-maximized");
            options.AddExcludedArgument("enable-automation");

            IWebDriver driver = new ChromeDriver(options);

            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(Timeout.Infinite));

            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl(@"https://web.whatsapp.com");

            wait.Until(drv => drv.FindElement(By.Id("side")));

            Thread.Sleep(5000);

            main_brain(driver, wait);

            while (true)
            {
                //encontrando the header/nombre del usuario
                string name_user = wait.Until(drv => drv.FindElement(By.XPath("//header[@class='_23P3O']//div[2]//div[1]//div[1]//span"))).Text;

                if (lst_user.ContainsKey(name_user))
                {
                    try
                    {
                        string contenido = driver.FindElement(By.XPath($"//div[contains(@aria-label, 'Lista de mensajes')]" +
                            $"//div[last()][contains(@class, 'message-in')]//span[contains(@class, 'i0jNr selectable-text copyable-text')]")).Text;

                        int num = Convert.ToInt32(contenido);

                        var textbox = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[2]"));

                        if (lst_user.GetValueOrDefault(name_user) == 0)
                        {
                            if (num == 1)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_1()}'", textbox);
                                lst_user[name_user] = 1;
                            }
                            else if (num == 2)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_2()}'", textbox);
                                lst_user[name_user] = 2;
                            }
                            else if (num == 3)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_3()}'", textbox);
                                lst_user[name_user] = 3;
                            }
                            else if (num == 4)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_4()}'", textbox);
                                lst_user[name_user] = 4;
                            }
                            else if (num == 0)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_0()}'", textbox);
                                lst_user.Remove(name_user);
                            }
                            else
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_error()}'", textbox);
                            }
                        }
                        else if (lst_user.GetValueOrDefault(name_user) > 0 && lst_user.GetValueOrDefault(name_user) < 4)
                        {
                            if (num == 9)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_menú()}'", textbox);
                                lst_user[name_user] = 0;
                            }
                            else if (num == 0)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_0()}'", textbox);
                                lst_user.Remove(name_user);
                            }
                            else
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_error()}'", textbox);
                            }
                        }
                        else if (lst_user.GetValueOrDefault(name_user) == 4)
                        {
                            if (num == 1)
                            {
                                string textbox_one = "/html/body/div[1]/div[1]/div[1]/div[2]/div[2]/span/div[1]/span/div[1]/div/div[2]/div/div[1]/div[3]/div/div/div[2]/div[1]/div[2]";

                                send_media(driver, wait, jse, mensaje_4_1(), textbox_one);

                                lst_user[name_user] = 1;

                                continue;
                            }
                            else if (num == 2)
                            {
                                string textbox_one = "/html/body/div[1]/div[1]/div[1]/div[2]/div[2]/span/div[1]/span/div[1]/div/div[2]/div/div[1]/div[3]/div/div[2]/div[1]/div[2]";

                                send_media(driver, wait, jse, mensaje_4_2(), textbox_one);

                                lst_user[name_user] = 1;

                                continue;
                            }
                            else if (num == 9)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_menú()}'", textbox);
                                lst_user[name_user] = 0;
                            }
                            else if (num == 0)
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_0()}'", textbox);
                                lst_user.Remove(name_user);
                            }
                            else
                            {
                                jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_error()}'", textbox);
                            }
                        }

                        textbox.SendKeys("." + Keys.Backspace + Keys.Enter);
                    }
                    catch
                    {
                        var textbox = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[2]"));

                        jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_error()}'", textbox);

                        textbox.SendKeys("." + Keys.Backspace + Keys.Enter);
                    }
                }
                else
                {
                    lst_user.Add(name_user, 0);

                    //encontrando el textbox
                    var write_message_0 = wait.Until(drv => drv.FindElement(By.XPath("//div[@class='_6h3Ps']" +
                        "//div[@class='_13NKt copyable-text selectable-text' and @dir='ltr']")));

                    jse.ExecuteScript($"arguments[0].innerHTML = '{mensaje_bienvenida()}'", write_message_0);

                    write_message_0.SendKeys("." + Keys.Backspace + Keys.Enter);
                }

                Thread.Sleep(2000);

                main_brain(driver, wait);
            }
        }

        static void send_media(IWebDriver driver, WebDriverWait wait, IJavaScriptExecutor jse, List<string> lst, string textbox)
        {
            wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[1]/div[2]/div/div"))).Click();

            var image = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[1]/div[2]/div/span/div[1]/div/ul/li[1]/button/input")));

            image.SendKeys(lst[0]);

            var new_textbox = wait.Until(drv => drv.FindElement(By.XPath(textbox)));

            jse.ExecuteScript($"arguments[0].innerHTML = '{lst[1]}'", new_textbox);

            new_textbox.SendKeys("." + Keys.Backspace + Keys.Enter);

            Thread.Sleep(2000);

            main_brain(driver, wait);
        }

        static string mensaje_bienvenida()
        {
            return "*¡Bienvenido! 🥳*<br>" +
                "Me llamo Nevirt, soy un Chat Bot, es un gusto 😊<br><br>" +
                mensaje_menú();
        }
        static string mensaje_menú()
        {
            return "1️⃣ ¿Quién es tu creador?<br>" +
                "2️⃣ ¿Cuantos años tienes?<br>" +
                "3️⃣ ¿Cuál es tu dirección?<br>" +
                "4️⃣ Ver promociones<br>" +
                "0️⃣ Adiós";
        }
        static string mensaje_1()
        {
            return "Mi creador es la empresa Primaus 😎🎉<br>" +
                "Estoy feliz de haber nacido 😃✨<br><br>" +
                "9️⃣ Vovler al menú<br>" +
                "0️⃣ Salir";
        }
        static string mensaje_2()
        {
            return "Tengo nada más 2 días de haner sido encendida pero, a pesar de mi corta edad eso sería inecesario tener en cuenta, ya que soy un bot<br><br>" +
                "9️⃣ Vovler al menú<br>" +
                "0️⃣ Salir";
        }
        static string mensaje_3()
        {
            return "Secreto, pero te enviaré la dirección de la estatua de la libertad, es grandisos por cierto 🤩<br><br>" +
                "9️⃣ Vovler al menú<br>" +
                "0️⃣ Salir";
        }
        static string mensaje_4()
        {
            return "Actualmente tenemos 2 promociones<br><br>" +
                "1️⃣ Tarjeta de Regalo 🤩✨<br>" +
                "2️⃣ Vídeo promocional 🥳🎉<br>" +
                "9️⃣ Volver al menú<br>" +
                "0️⃣ Salir";
        }
        static List<string> mensaje_4_1()
        {
            var lst = new List<string>()
            {
                "C:\\Users\\marti\\Pictures\\god_shaggy.jpg",
                "9️⃣ Vovler al menú<br>" +
                "0️⃣ Salir"
        };

            return lst;
        }
        static List<string> mensaje_4_2()
        {
            var lst = new List<string>()
            {
                "E:\\videos youtube\\videos memes\\Pantalla de colores meme.mp4",
                "9️⃣ Vovler al menú<br>" +
                "0️⃣ Salir"
            };

            return lst;
        }
        static string mensaje_0()
        {
            return "Muchas gracias por tu tiempo 😊<br>" +
                "Espero que nos volvamos a encontrar pronto 😃✨";
        }
        static string mensaje_error()
        {
            return "Lo lamento, no le he entendido ☹<br>" +
                "Vuelva a intentarlo por favor";
        }
    }
}
