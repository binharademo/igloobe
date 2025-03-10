using System;

namespace Br.Com.IGloobe.Connector.Mote {
    public class StateChangedEventArgs: EventArgs {

		public MotionState MotionState;

		public StateChangedEventArgs(MotionState ws) {
			MotionState = ws;
		}
	}
}