using NullSoftware.Models;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace NullSoftware.Nodes
{
    internal class InputFileNode : NodeViewModel
    {
        public ValueNodeOutputViewModel<Texture?> OutTexture { get; }

        public Texture Value { get; }

        static InputFileNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new Views.InputFileNodeView(), typeof(IViewFor<InputFileNode>));
        }

        public InputFileNode(Texture texture)
        {
            Name = "Input File";

            Value = texture;
            OutTexture = new ValueNodeOutputViewModel<Texture?>() { Name = "Texture", Value = Observable.Return(texture) };
            Outputs.Add(OutTexture);
        }

    }
}
