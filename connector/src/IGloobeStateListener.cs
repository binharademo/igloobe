/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file defines the IGloobeStateListener interface, which is used to 
 * notify listeners about connection state changes in the Igloobe system.
 */
namespace Br.Com.IGloobe.Connector{

    public interface IGloobeStateListener {
        void StateChanched(ConnectionState toState);
    }

}
