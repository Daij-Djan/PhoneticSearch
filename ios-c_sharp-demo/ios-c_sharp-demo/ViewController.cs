using System;
using System.Diagnostics;
using UIKit;
using PhoneticSearch;

namespace iosc_sharpdemo
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            Debug.WriteLine("levenstein: " + "1".levensteinDistanceTo("11"));
            Debug.WriteLine("Metaphone2: " + "Hausdach".metaphone2Code());
            Debug.WriteLine("tokens: " + "tokens!!! of thiss$".normalizedTokens());
            Debug.WriteLine("soundex: " + "Hausdach".soundexCode());

			String fuzzyInput = "The house is maintained by aHAausritters while we are on vacation"; //input is something that is close to/sounds like Housesitter
            String[] knownWords = { "house sitter", "baby sitter", "gardener" };
        
            PhoneticMatch match = PhoneticSearch.PhoneticSearch.FindTermInString(fuzzyInput, knownWords, PhoneticMatchAlgorithm.Metaphone2, PhoneticSearch.PhoneticSearch.DefaultDistance);
        
            if(match != null) {
                Debug.WriteLine("We found " + match.term + " for the input " + fuzzyInput + ". It has a levensteindistance of " + match.distance);
            }
            else {
                Debug.WriteLine(@"nothing found for input " + fuzzyInput);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
