using System.Collections.Generic;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IMagasinService
    {
        IEnumerable<CaissePhotoSimple> DonneCaissesPhotoSimples();
    }
}
