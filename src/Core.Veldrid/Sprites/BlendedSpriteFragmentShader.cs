﻿using System.Diagnostics.CodeAnalysis;
using VeldridGen.Interfaces;

namespace UAlbion.Core.Veldrid.Sprites;

[Name("BlendedSpriteSF.frag")]
[Input(0, typeof(BlendedSpriteIntermediateData))]
[ResourceSet(0, typeof(CommonSet))]
[ResourceSet(1, typeof(SpriteSet))]
[Output(0, typeof(SimpleFramebuffer))]
[SuppressMessage("Microsoft.Naming", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used for code generation")]
internal partial class BlendedSpriteFragmentShader : IFragmentShader { }