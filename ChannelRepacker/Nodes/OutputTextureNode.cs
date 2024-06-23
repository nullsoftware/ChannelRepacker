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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NullSoftware.Nodes
{
    public class OutputTextureNode : NodeViewModel
    {
        public ValueNodeInputViewModel<int> Width { get; }
        public ValueNodeInputViewModel<int> Height { get; }
        public ValueNodeInputViewModel<TextureChannel> R { get; }
        public ValueNodeInputViewModel<TextureChannel> G { get; }
        public ValueNodeInputViewModel<TextureChannel> B { get; }
        public ValueNodeInputViewModel<TextureChannel> A { get; }

        public OutputTexture OutputTexture { get; }

        static OutputTextureNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new Views.OutputTextureNodeView(), typeof(IViewFor<OutputTextureNode>));
        }

        public OutputTextureNode(OutputTexture outputTexture)
        {
            this.Name = "Output Texture";
            CanBeRemovedByUser = false;
            OutputTexture = outputTexture;

            Width = new ValueNodeInputViewModel<int>() { Name = "Width" };
            Height = new ValueNodeInputViewModel<int>() { Name = "Height" };

            R = new ValueNodeInputViewModel<TextureChannel>() { Name = "R" };
            G = new ValueNodeInputViewModel<TextureChannel>() { Name = "G" };
            B = new ValueNodeInputViewModel<TextureChannel>() { Name = "B" };
            A = new ValueNodeInputViewModel<TextureChannel>() { Name = "A" };
            
            Inputs.Add(Width);
            Inputs.Add(Height);
            Inputs.Add(R);
            Inputs.Add(G);
            Inputs.Add(B);
            Inputs.Add(A);

            Width.ValueChanged.Subscribe(_ => Refresh());
            Height.ValueChanged.Subscribe(_ => Refresh());
            R.ValueChanged.Subscribe(_ => Refresh());
            G.ValueChanged.Subscribe(_ => Refresh());
            B.ValueChanged.Subscribe(_ => Refresh());
            A.ValueChanged.Subscribe(_ => Refresh());
        }

        private void Refresh()
        {
            int width = Width.Value;
            int height = Height.Value;
            TextureChannel rChannel = R.Value ?? FakeChannel.MinValue;
            TextureChannel gChannel = G.Value ?? FakeChannel.MinValue;
            TextureChannel bChannel = B.Value ?? FakeChannel.MinValue;
            TextureChannel aChannel = A.Value ?? FakeChannel.MaxValue;

            OutputTexture.Generate(width, height, rChannel, gChannel, bChannel, aChannel);
        }



    }
}
