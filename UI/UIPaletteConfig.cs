//
//    Copyright 2023-2024 BasicallyIAmFox
//
//    Licensed under the Apache License, Version 2.0 (the "License")
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using AnyPaletteShader.Graphics;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace AnyPaletteShader.UI;

public sealed class UIPaletteConfig : UIState {
#if DEBUG
	// Just so we can debug it easily.
	internal static UIPaletteConfig Instance => new();
#else
	internal static readonly UIPaletteConfig Instance = new();
#endif

	[MaybeNull] private UIElement main;
	[MaybeNull] private UIPanel panel;
	[MaybeNull] private UITextPanel<string> backButton;

	[MaybeNull] private UIPanel previewPanel;
	[MaybeNull] private UIPanel previewInnerPanel;
	[MaybeNull] private UIImageWithShader previewIcon;

	public override void OnInitialize() {
		main = new UIElement {
			Width           = StyleDimension.FromPercent         (       0.80f),
			MaxWidth        = StyleDimension.FromPixels          (+600f       ),
			Top             = StyleDimension.FromPixels          (+160f       ),
			Height          = StyleDimension.FromPixelsAndPercent(-160f, 1.00f),
			HAlign          = 0.50f
		};
		Append(main);

		panel = new UIPanel {
			Width           = StyleDimension.FromPercent         (       1.00f),
			Height          = StyleDimension.FromPixelsAndPercent(-160f, 0.80f),
			Top             = StyleDimension.FromPixels          (+80f        ),
			BackgroundColor = UICommon.MainPanelBackground
		};
		main.Append(panel);

		var palettePanel = new UIPanel {
			Width           = StyleDimension.FromPixelsAndPercent(-162f, 1.00f),
			Height          = StyleDimension.FromPixelsAndPercent(-1f  , 1.00f),
			Top             = StyleDimension.FromPixels          (-1f         ),
			BackgroundColor = UICommon.MainPanelBackground
		};
		panel.Append(palettePanel);
		var paletteText = new UIText(AnyPaletteShader.Instance.GetLocalization("UI.PaletteConfigButton")) {
			HAlign          = 0.50f
		};
		palettePanel.Append(paletteText);

		InitializePreviewPanel();

		backButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigBack")) {
			Width           = StyleDimension.FromPixelsAndPercent(-10f, 0.50f),
			Height          = StyleDimension.FromPixels          (+25f       ),
			Top             = StyleDimension.FromPixels          (+20f       ),
			HAlign          = 0.50f,
			VAlign          = 0.70f
		};
		backButton.WithFadedMouseOver();
		backButton.OnLeftClick += BackClick;
		main.Append(backButton);
	}

	private void InitializePreviewPanel() {
		previewPanel = new UIPanel {
			Width           = StyleDimension.FromPixels          (+158f       ),
			Height          = StyleDimension.FromPixelsAndPercent(-1f  , 1.00f),
			Top             = StyleDimension.FromPixels          (-1f         ),
			HAlign          = 1.00f,
			BackgroundColor = UICommon.MainPanelBackground
		};
		previewPanel.SetPadding(5f);
		panel!.Append(previewPanel);

		// Preview Icon
		previewInnerPanel = new UIPanel {
			Width           = StyleDimension.FromPercent         (       1.00f),
			Height          = StyleDimension.FromPercent         (       1.00f),
			MaxWidth        = StyleDimension.FromPixels          (+160f       ),
			MaxHeight       = StyleDimension.FromPixels          (+150f       ),
			BackgroundColor = UICommon.DefaultUIBlue * 0.5f
		};
		previewInnerPanel.SetPadding(6f);

		previewIcon = new UIImageWithShader(UIAssets.PalettePreview, PaletteShader.Instance) {
			Width           = StyleDimension.FromPercent         (       1.00f),
			Height          = StyleDimension.FromPercent         (       1.00f),
			ScaleToFit      = true
		};

		previewPanel.Append(previewInnerPanel);
		previewInnerPanel.Append(previewIcon);
	}

	private static void BackClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(in SoundID.MenuClose);

		AnyPaletteShader.ApplyPaletteShader = true;

		Main.menuMode = Interface.modsMenuID;
	}
}
