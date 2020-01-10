using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AddNewFavourite.xaml
    /// </summary>
    public partial class AddNewFavourite : Window
    {
        public User currentUser;
        public IQueryable<Favorite> favList;
        public List<MovieSpecificDetails> movieDetails;
        StreamingEntities context = new StreamingEntities();

        //Constructor som tager User instans som parameter værdi
        public AddNewFavourite(User user)
        {
            InitializeComponent();

            // Centrere vinduet 
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;


            currentUser = user;

            // Henter Favourite objekter som passer med brugerens Id og sætter favList lig med det der bliver retuneret
            favList = from favs in context.Favorites
                      where favs.UserId == currentUser.Id
                      select favs;

            UpdateMovieList();
        }

        // Følgende funktion opdatere Listen over film 
        public void UpdateMovieList()
        {
            // Henter info om Film og sætter movieDetails lig med det der bliver retuneret 
            movieDetails = (from fD in context.FilmDetails
                            join f in context.Films on fD.FilmId equals f.Id
                            select new MovieSpecificDetails
                            {
                                Titel = f.Titel,
                                Genre = f.Genre,
                                Rating = f.Rating,
                                Release = f.Release,
                                Playtime = fD.Playtime,
                                Id = f.Id
                            })
                            /* filtrere listen af film så vi ikke får film som brugeren 
                               allerede har valgt at tilføje som favorit */
                            .Where(m => !favList.Any(f => m.Id == f.FilmId))
                            .ToList();

            // Opdatere vores ListView med film detaljerne
            movieDetailsView.ItemsSource = movieDetails;

        }


        // Følgende bliver kørt når der bliver trykket på ChooseFavourites knappen 
        private void ChooseFavourite_Click(object sender, RoutedEventArgs e)

        {
            // Laver en instans af Favorite klassen
            Favorite favItem = new Favorite();
            favItem.UserId = currentUser.Id;
            /* Får fat på FilmId, ved at tilgå Tag elementet på den knap som invokede det her event 
             og da knappens Tag element er binded til FilmId'et, så kan vi tilgå det den vej og parse det til Int*/
            favItem.FilmId = Int32.Parse(((Button)sender).Tag.ToString());

            // Tilføjer favItem til Favorites tabellen i database og gemmer ændringerne. 
            context.Favorites.Add(favItem);
            context.SaveChanges();

            /* Efter det opdatere vi film listen så den ikke kommer til 
             at indholde den film brugeren lige har tilføjet til favoritter */
            UpdateMovieList();

            // Giver en notifikation om at filmen er tilføjet til favoritter
            System.Windows.MessageBox.Show("Movie added to favourites");
        }


        /* Når der bliver klikket på BackToFavourites knappen så lukker vi det nuværende vindue og viser 
      UserFavourites vinduet 
     */
        public void BackToFovourites_Click(object sender, RoutedEventArgs e)
        {
            UserFavourites favPage = new UserFavourites(currentUser);
            this.Hide();
            favPage.Show();
        }


        /* Når der bliver klikket på LogOffButton knappen så lukker vi det nuværende vindue og viser 
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
