using BepuPhysics;
using Microsoft.Xna.Framework;
using SE2.Utilities;
using SVec3 = System.Numerics.Vector3;

namespace SE2.Physics
{
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public SVec3 Gravity;
        private SVec3 _gravityDt;

        public PoseIntegratorCallbacks(SVec3 gravity) : this()
        {
            Gravity = gravity;
        }
        
        public void Initialize(Simulation simulation)
        {
            
        }

        public void PrepareForIntegration(float dt)
        {
            _gravityDt = Gravity * dt;
        }

        public void IntegrateVelocity(int bodyIndex, in RigidPose pose, in BodyInertia localInertia, int workerIndex,
            ref BodyVelocity velocity)
        {
            if (localInertia.InverseMass > 0)
                velocity.Linear = velocity.Linear + _gravityDt;
        }

        public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;
    }
}