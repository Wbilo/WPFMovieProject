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
    /// Interaction logic for UserFavourites.xaml
    /// </summary>
    public partial class UserFavourites : Window
    {
        public IQueryable<Film> filmList;
        public User currentUser;
        public IQueryable<Favorite> favList;
        StreamingEntities context = new StreamingEntities();

        //Constructor som tager User instans som parameter værdi
        public UserFavourites(User user)
        {
            InitializeComponent();

            // Centrere vinduet 
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            currentUser = user;

            // henter film som brugeren har valgt at favourite og sætter filmList lig med det der bliver retuneret
            filmList = from favs in context.Favorites
                       join films in context.Films on favs.FilmId equals films.Id
                       where favs.UserId == currentUser.Id
                       select films;
            // Opdatere combobox med film info    
            filmBox.ItemsSource = filmList.ToList();

            filmBox.DisplayMemberPath = "Titel";
            filmBox.SelectedValuePath = "Id";


        }

        private void filmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            actorDetailsView.ItemsSource = null;


            // Får værdien fra den valgte item i comboboxen, hvilket matcher ID'et på den valgte film
            var selectedFilm = filmBox.SelectedValue;



            // Får filmen fra filmList som matcher ID'et på den valgte film
            var filmChosen = filmList.FirstOrDefault(x => x.Id.Equals((int)selectedFilm));

            // populate textboxes med film info  
            genre.Text = filmChosen.Genre.ToString();
            rating.Text = filmChosen.Rating.ToString();
            release.Text = filmChosen.Release.ToString();
            expired.Text = filmChosen.Expired.ToString();

            // Henter actor details for den valgte film  
            updateActorDetails((int)selectedFilm);
        }

        // Følgende funktion opdatere info om skuespillerne 
        private void updateActorDetails(int filmID)
        {

            // joiner de forskellige database set sammen, for at få fat i de properties jeg skal bruge.  
            var actorDetails = from actorDet in context.ActorRoles
                               join actors in context.Actors on actorDet.ActorId equals actors.Id
                               where actorDet.FilmId == filmID
                               select new { actorDet.RoleName, actors.Name, actorDet.ActorId };

            // populater min ListView med dataet 
            actorDetailsView.ItemsSource = actorDetails.ToList();

        }

        /* Når der bliver klikket på FavButton så lukker vi det nuværende vindue og viser 
         AddNewFavourite vinduet
        */
        private void FavButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewFavourite newFavsWindow = new AddNewFavourite(currentUser);
            this.Hide();
            newFavsWindow.Show();


        }

        /* Når der bliver klikket på LogOffButton så lukker vi det nuværende vindue og viser 
       UserLogin vinduet 
      */
        public void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            UserLogin loginPage = new UserLogin();
            this.Hide();
            loginPage.Show();
        }
    }
}
