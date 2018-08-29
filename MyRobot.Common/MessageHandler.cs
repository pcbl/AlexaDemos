using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using MyRobot.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MyRobot
{
    public class MessageHandler
    {
        CompositionContainer _container;

        [ImportMany]
        private IEnumerable<Lazy<IMessage, IMessageIntent>> _handlers = null;

        public MessageHandler()
        {
            #region MEF
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();
            //For now we will get only this assembly(MyRobot.Common)
            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            //That means: _handlers!
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
            #endregion
        }

        public SkillResponse Handle(IntentRequest request)
        {
            foreach (var i in _handlers)
            {
                if (i.Metadata.Intent.Equals(request.Intent.Name))
                    return i.Value.Process(request);
            }
            return ResponseBuilder.Tell("No Handler found for "+ request.Intent.Name);
        }
    }
}
