using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ExpressionTreeIssue.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return View(new Model {
                Rows = new EntityCollection<RowEntity> {
                    new RowEntity {
                        Id = 1,
                        Cells = new EntityCollection<CellEntity> {
                            new CellEntity { Id = 1 },
                            new CellEntity { Id = 2 }
                        }
                    },
                    new RowEntity {
                        Id = 2,
                        Cells = new EntityCollection<CellEntity> {
                            new CellEntity { Id = 1 }
                        }
                    }
                }
            });
        }
    }

    public interface IModel { }

    public interface IEntityCollection<T> : ICollection<T> where T : IEntity { }

    public interface IEntity
    {
        int Id { get; }
    }

    public class Model : IModel
    {
        public EntityCollection<RowEntity> Rows { get; set; }
    }

    public class EntityCollection<T> : List<T>, IEntityCollection<T> where T : IEntity { }

    public class RowEntity : IEntity
    {
        public int Id { get; set; }
        public EntityCollection<CellEntity> Cells { get; set; }
    }

    public class CellEntity : IEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}