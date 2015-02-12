using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using gw2api.Request;
using ReactiveUI;

namespace PromotionViabilityWpf.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ReactiveList<Task> ActiveTasks;

        private readonly ObservableAsPropertyHelper<Boolean> loading; 
        public Boolean IsLoading
        {
            get { return loading.Value; }
        }

        internal MainWindowViewModel()
        {
            ActiveTasks = new ReactiveList<Task>();

            this.WhenAnyValue(x => x.ActiveTasks.Count)
                .Select(x => x != 0)
                .ToProperty(this, x => x.IsLoading, out loading);
        }

        public async void Load<TKey, TValue>(IEnumerable<IBundlelable<TKey, TValue>> bundlelable)
        {
            var command = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var task = Bundler.BundleAndSet(bundlelable);
                ActiveTasks.Add(task);
                await task;

                ActiveTasks.Remove(task);;
            });
            await command.ExecuteAsync();
        }

    }
}
