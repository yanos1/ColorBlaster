// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("vwEeLeO4wgGXoXeJkJFGsOyPBT3KePvYyvf889B8snwN9/v7+//6+Xj79frKePvw+Hj7+/o/YaFq+SQ58WsbXcLsku7Yfc73a8aB6bYHtkD+mC8elcSA2j5iDm/IG8vL0+BpVUMhmG6N07EoMwJy9yHny1f8E9x4H/1gSlJIwXVHB4m5SXQO+hbWwQz9zSMF02kh9qx8ewetaDz7RYhb3QjFPrgZgzTa3BWcmpqHzS5Qh5XPpL9En6Knl4pSLpl2PAJhoxi+BSVXAWHYlYgdOODbuNMi24tLui8T0ilFVobOxC/c3WI1JXP9bdvRrmZI5xc04pr6j6+RAiZcozIYpOxXQoqyd8OMuasncKxSz1gsTEvTOwB4Rc2P6iy2WzyXKfj5+/r7");
        private static int[] order = new int[] { 10,10,10,12,8,9,7,13,10,13,10,13,12,13,14 };
        private static int key = 250;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
