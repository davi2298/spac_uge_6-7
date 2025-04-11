using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagersystem.Entitys;

public interface AEntity<T>
{
        public void Update(T entity);

}
