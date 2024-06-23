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
using System.Windows.Media.Imaging;

namespace NullSoftware.Nodes
{
    public class TextureNode : NodeViewModel
    {
        public ValueNodeInputViewModel<Texture?> InTexture { get; }

        public ValueNodeOutputViewModel<int> Width { get; }
        public ValueNodeOutputViewModel<int> Height { get; }

        public ValueNodeOutputViewModel<TextureChannel?> R { get; }
        public ValueNodeOutputViewModel<TextureChannel?> G { get; }
        public ValueNodeOutputViewModel<TextureChannel?> B { get; }
        public ValueNodeOutputViewModel<TextureChannel?> A { get; }

        static TextureNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new Views.TextureNodeView(), typeof(IViewFor<TextureNode>));
        }

        public TextureNode()
        {
            Name = "Texture";
            InTexture = new ValueNodeInputViewModel<Texture?>() { Name = "In" };
            Inputs.Add(InTexture);

            Width = new ValueNodeOutputViewModel<int>() { Name = "Width", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t?.Width ?? 0) };
            Height = new ValueNodeOutputViewModel<int>() { Name = "Height", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t?.Height ?? 0) };
            R = new ValueNodeOutputViewModel<TextureChannel?>() { Name = "R", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t != null ? new RedChannel(t) : null) };
            G = new ValueNodeOutputViewModel<TextureChannel?>() { Name = "G", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t != null ? new GreenChannel(t) : null) };
            B = new ValueNodeOutputViewModel<TextureChannel?>() { Name = "B", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t != null ? new BlueChannel(t) : null) };
            A = new ValueNodeOutputViewModel<TextureChannel?>() { Name = "A", Value = this.WhenAnyObservable(vm => vm.InTexture.ValueChanged).Select(t => t != null ? new AlphaChannel(t) : null) };

            Outputs.Add(Width);
            Outputs.Add(Height);
            Outputs.Add(R);
            Outputs.Add(G);
            Outputs.Add(B);
            Outputs.Add(A);
        }
    }
}
