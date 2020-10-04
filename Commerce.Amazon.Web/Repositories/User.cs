namespace Commerce.Amazon.Web.Repositories
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string UserId { get; set; }
        public string UserGuid { get; set; }
        public int State { get; set; }
    }

}
