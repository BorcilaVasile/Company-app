using Administrare_firma.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrare_firma.MVVM.ViewModel
{
    public class ProjectsViewModel : ObservableObject
    {
        private ObservableCollection<Projects> _projects;
        public ObservableCollection<Projects> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }
        public ProjectsViewModel() {
            LoadProjects();

        }
        private void LoadProjects()
        {
            using (var context = new ApplicationDbContext()) 
            {
                    var projectsFromDb = context.Projects.ToList();
                    Projects = new ObservableCollection<Projects>(projectsFromDb);
            }
        }
    }
}
