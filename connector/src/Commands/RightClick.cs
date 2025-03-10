/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file implements the RightClick class that provides functionality 
 * for handling right-click operations in the Igloobe system. It manages 
 * the state of right-click actions triggered by input devices.
 */
namespace Br.Com.IGloobe.Connector.Commands {

    public class RightClick {

        private static bool _isRightClick ;
        public static void PrepareIt() { _isRightClick = true; }
        public static void Released() { _isRightClick = false; }
        public static bool IsRightClick() { return _isRightClick; }
    }
}