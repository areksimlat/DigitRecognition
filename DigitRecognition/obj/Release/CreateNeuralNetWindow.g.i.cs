﻿#pragma checksum "..\..\CreateNeuralNetWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "03AF1D718DB0B12F544E38CD14951409"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using DigitRecognition;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DigitRecognition {
    
    
    /// <summary>
    /// CreateNeuralNetWindow
    /// </summary>
    public partial class CreateNeuralNetWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 50 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox inputNeuronTextBox;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox outputNeuronTextBox;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addLayerButton;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removeLayerButton;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button okButton;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\CreateNeuralNetWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid HiddenLayersGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DigitRecognition;component/createneuralnetwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CreateNeuralNetWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.inputNeuronTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.outputNeuronTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.addLayerButton = ((System.Windows.Controls.Button)(target));
            
            #line 81 "..\..\CreateNeuralNetWindow.xaml"
            this.addLayerButton.Click += new System.Windows.RoutedEventHandler(this.addLayerButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.removeLayerButton = ((System.Windows.Controls.Button)(target));
            
            #line 91 "..\..\CreateNeuralNetWindow.xaml"
            this.removeLayerButton.Click += new System.Windows.RoutedEventHandler(this.removeLayerButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.okButton = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\CreateNeuralNetWindow.xaml"
            this.okButton.Click += new System.Windows.RoutedEventHandler(this.okButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.HiddenLayersGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

