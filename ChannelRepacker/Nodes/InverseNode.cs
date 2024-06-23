using NullSoftware.Models;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Nodes
{
    public class InverseNode : NodeViewModel
    {
        public ValueNodeInputViewModel<TextureChannel> In { get; }
        public ValueNodeOutputViewModel<TextureChannel> Out { get; }

        static InverseNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new Views.InverseNodeView(), typeof(IViewFor<InverseNode>));
        }

        public InverseNode()
        {
            this.Name = "Inverse";

            In = new ValueNodeInputViewModel<TextureChannel>() { Name = "In" };
            Inputs.Add(In);

            Out = new ValueNodeOutputViewModel<TextureChannel>()
            {
                Name = "Out",
                Value = this.WhenAnyObservable(vm => vm.In.ValueChanged).Select(b => b != null ? new InverseChannel(b) : null),
            };
            Outputs.Add(Out);
        }
    }
}
