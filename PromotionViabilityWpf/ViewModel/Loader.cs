using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gw2api.Request;

namespace PromotionViabilityWpf.ViewModel
{
    public class Loader
    {
        private List<Task> ActiveTasks;

        public Boolean IsLoading
        {
            get { return ActiveTasks.Count > 0; }
        }

        internal Loader()
        {
            ActiveTasks = new List<Task>();
        }

        public void Load<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
        {
            var task = Bundler.BundleAndSet(bundlelable);
            ActiveTasks.Add(task);

            task.ContinueWith(t =>
            {
                ActiveTasks.Remove(task);
            });
        }

    }
}
