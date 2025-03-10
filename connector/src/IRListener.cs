/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file defines the IRListener interface used for receiving infrared 
 * state change notifications from Igloobe devices. It provides a callback method 
 * for when the IR sensor state changes or lights are toggled.
 */
using Br.Com.IGloobe.Connector.Mote;

namespace Br.Com.IGloobe.Connector {

    public interface IRListener {
        void StateChanged(bool lightsOn, IrState irState);
    }
}
