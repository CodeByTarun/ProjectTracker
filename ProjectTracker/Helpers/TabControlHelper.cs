using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectTracker.Helpers
{
    public static class TabControlHelper
    {
        private static ListBox TabListBox;

        public static void SetTabListBox(ListBox tabListBox)
        {
            TabListBox = tabListBox;
        }

        public static void AddTab(Project projectToAdd)
        {
            if (CheckIfProjectIsInTabListBox(projectToAdd))
            {
                TabListBox.SelectedItem = ((ObservableCollection<Project>)TabListBox.ItemsSource).Where(p => p.Id == projectToAdd.Id).FirstOrDefault();
            } else
            {
                if (((Project)(TabListBox.SelectedItem)).Id == 0)
                {
                    int index = TabListBox.SelectedIndex;
                    ((ObservableCollection<Project>)TabListBox.ItemsSource)[index] = projectToAdd;
                    TabListBox.SelectedIndex = index;
                }
                else
                {
                    ((ObservableCollection<Project>)TabListBox.ItemsSource).Add(projectToAdd);
                    TabListBox.SelectedItem = ((ObservableCollection<Project>)TabListBox.ItemsSource).Last();
                }
            }
        }
        public static void RemoveTab(Project projectToRemove)
        {
            Project _projectToRemove = ((ObservableCollection<Project>)TabListBox.ItemsSource).Where(p => p.Id == projectToRemove.Id).FirstOrDefault();

            if (CheckIfProjectIsInTabListBox(_projectToRemove))
            {
                if((TabListBox.SelectedItem) == _projectToRemove)
                {
                    ChangeSelectedItem(_projectToRemove);
                }
                ((ObservableCollection<Project>)TabListBox.ItemsSource).Remove(_projectToRemove);
            }
        }
        public static void EditTab(Project projectToEdit)
        {
            if (CheckIfProjectIsInTabListBox(projectToEdit))
            {
                Project project = ((ObservableCollection<Project>)TabListBox.ItemsSource).Where(p => p.Id == projectToEdit.Id).FirstOrDefault();

                if (project != null)
                {
                    int index = ((ObservableCollection<Project>)TabListBox.ItemsSource).IndexOf(project);

                    if (project == TabListBox.SelectedItem)
                    {
                        ((ObservableCollection<Project>)TabListBox.ItemsSource)[index] = projectToEdit;
                        TabListBox.SelectedIndex = index;
                    }

                    ((ObservableCollection<Project>)TabListBox.ItemsSource)[index] = projectToEdit;
                }
            }
        }

        private static bool CheckIfProjectIsInTabListBox(Project projectToCheck)
        {
            if (projectToCheck == null)
            {
                return false;
            }
            return ((ObservableCollection<Project>)TabListBox.ItemsSource).Any(p => p.Id == projectToCheck.Id);
        }
        private static void ChangeSelectedItem(Project projectToRemove)
        {
            int index = ((ObservableCollection<Project>)TabListBox.ItemsSource).IndexOf(projectToRemove);

            if (index < ((ObservableCollection<Project>)TabListBox.ItemsSource).Count())
            {
                TabListBox.SelectedItem = ((ObservableCollection<Project>)TabListBox.ItemsSource).ElementAt(index);
            }
            else if ((index - 1) >= 0)
            {
                TabListBox.SelectedItem = ((ObservableCollection<Project>)TabListBox.ItemsSource).ElementAt(index - 1);
            } 
        }

        
    }
}
