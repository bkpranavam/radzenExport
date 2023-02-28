using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Testgridexport.Data;

namespace Testgridexport
{
    public partial class TestService
    {
        TestContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly TestContext context;
        private readonly NavigationManager navigationManager;

        public TestService(TestContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportEmplyeeTablesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/test/emplyeetables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/test/emplyeetables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmplyeeTablesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/test/emplyeetables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/test/emplyeetables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmplyeeTablesRead(ref IQueryable<Testgridexport.Models.Test.EmplyeeTable> items);

        public async Task<IQueryable<Testgridexport.Models.Test.EmplyeeTable>> GetEmplyeeTables(Query query = null)
        {
            var items = Context.EmplyeeTables.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmplyeeTablesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmplyeeTableGet(Testgridexport.Models.Test.EmplyeeTable item);

        public async Task<Testgridexport.Models.Test.EmplyeeTable> GetEmplyeeTableById(string id)
        {
            var items = Context.EmplyeeTables
                              .AsNoTracking()
                              .Where(i => i.Id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnEmplyeeTableGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmplyeeTableCreated(Testgridexport.Models.Test.EmplyeeTable item);
        partial void OnAfterEmplyeeTableCreated(Testgridexport.Models.Test.EmplyeeTable item);

        public async Task<Testgridexport.Models.Test.EmplyeeTable> CreateEmplyeeTable(Testgridexport.Models.Test.EmplyeeTable emplyeetable)
        {
            OnEmplyeeTableCreated(emplyeetable);

            var existingItem = Context.EmplyeeTables
                              .Where(i => i.Id == emplyeetable.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.EmplyeeTables.Add(emplyeetable);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(emplyeetable).State = EntityState.Detached;
                throw;
            }

            OnAfterEmplyeeTableCreated(emplyeetable);

            return emplyeetable;
        }

        public async Task<Testgridexport.Models.Test.EmplyeeTable> CancelEmplyeeTableChanges(Testgridexport.Models.Test.EmplyeeTable item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmplyeeTableUpdated(Testgridexport.Models.Test.EmplyeeTable item);
        partial void OnAfterEmplyeeTableUpdated(Testgridexport.Models.Test.EmplyeeTable item);

        public async Task<Testgridexport.Models.Test.EmplyeeTable> UpdateEmplyeeTable(string id, Testgridexport.Models.Test.EmplyeeTable emplyeetable)
        {
            OnEmplyeeTableUpdated(emplyeetable);

            var itemToUpdate = Context.EmplyeeTables
                              .Where(i => i.Id == emplyeetable.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(emplyeetable);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmplyeeTableUpdated(emplyeetable);

            return emplyeetable;
        }

        partial void OnEmplyeeTableDeleted(Testgridexport.Models.Test.EmplyeeTable item);
        partial void OnAfterEmplyeeTableDeleted(Testgridexport.Models.Test.EmplyeeTable item);

        public async Task<Testgridexport.Models.Test.EmplyeeTable> DeleteEmplyeeTable(string id)
        {
            var itemToDelete = Context.EmplyeeTables
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmplyeeTableDeleted(itemToDelete);


            Context.EmplyeeTables.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmplyeeTableDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}