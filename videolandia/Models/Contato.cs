using System.ComponentModel.DataAnnotations;

namespace videolandia.Models
{
    public class Contato
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required]
        public string NomeContato { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Assunto")]
        public string Assunto { get; set; }

        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; }
    }
}
