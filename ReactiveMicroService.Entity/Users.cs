using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.Entity
{
    //https://www.daveops.co.in/post/code-first-entity-framework-core-mysql#:~:text=NET%206.0%3A%20Code%20First%20with%20Entity%20Framework%20Core%20and%20MySQL,-Updated%3A%20Jan%2017&text=Entity%20Framework%20(EF)%20Core%20is,code%20for%20accessing%20any%20data.%20.
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }
    }
}
