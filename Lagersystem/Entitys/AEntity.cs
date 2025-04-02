using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagersystem.Entitys;

public abstract class AEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; init;}

    public AEntity(string id){
        Id = id;
    }
}
