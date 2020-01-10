using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFNetfluxProj
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        public UserLogin()
        {
            // Centrere vinduet 
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();
        }

        // Følgende funktion bliver kørt når der bliver trykket på LogonButton 
        private void LogonButton_Click(object sender, RoutedEventArgs e)
        {
            // using sørger for at i slutningen af scoped så skilder vi os af med context objektet
            using (var context = new StreamingEntities())
            {
                /* Prøver at få fat i en user fra Users tabellen, som matcher
                 de login credentials som der bliver indtastet i input felterne */
                var user = (from User u in context.Users
                            where String.Compare(u.UserName, username.Text) == 0
                            && String.Compare(password.Password, u.Password) == 0
                            select u).FirstOrDefault();

                /* hvis user ikke er null, altså hvis der bliver fundet en user der matcher, så
                 så lukker vi det nuværende vindue og viser   UserFavourites vinduet med den valgte brugers favorit film 
                 */
                if (user != null)
                {

                    UserFavourites fav = new UserFavourites(user);
                    this.Hide();
                    fav.Show();
                }
                // Ellers informer brugeren om at han/hun har indtastet forkerte credentials 
                else
                {
                    System.Windows.MessageBox.Show("Login failed, incorrect Password or Username");
                }
            }
        }
    }
}

