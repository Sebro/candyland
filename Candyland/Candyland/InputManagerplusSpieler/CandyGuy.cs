﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Candyland
{
    class CandyGuy : Playable
    {
        bool istargeting;
        Vector3 target;
        Effect effect;
        Texture2D texture;
        

        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker)
        {
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            this.m_position = position;
            this.direction = direction;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100, m_updateInfo);
            this.currentspeed = 0;
            this.gravity = -0.004f;
            this.upvelocity = 0;
        }

        public override void isNotCollidingWith(GameObject obj){ }

        public override void hasCollidedWith(GameObject obj){ }

        public override void update() { }

        public override void initialize(){ }

        public override void load(ContentManager content)
        {
            effect = content.Load<Effect>("Toon");
            texture = content.Load<Texture2D>("spielertextur");
            m_model = content.Load<Model>("spielerneu");
            calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        public override void jump()
        {
            if (isonground)
            {
                upvelocity = 0.08f;
                isonground = false;
            }
        }

        public override void moveTo(Vector3 goalpoint)
        {
            istargeting = true;
            target = goalpoint;
        }

        public override void movementInput(float movex, float movey, float camx, float camy)
        {
            if (istargeting)
            {
                float dx = target.X - m_position.X;
                float dz = target.Z - m_position.Z;
                float length = (float)Math.Sqrt(dx * dx + dz * dz);
                move(0.8f * dx / length, 0.8f * dz / length);
                if (length < 1) istargeting = false;
                fall();
            }
            else
            {
                fall();
                move(movex, movey);
                cam.changeAngle(camx, camy);
            }
        }

        private void fall() 
        {

            upvelocity += gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
            cam.changeposition(m_position);
        }

        #region collision

        public override void collide(GameObject obj)
        {
            cam.collideWith(obj);

            if (obj.GetType() == typeof(Platform)) collideWithPlatform(obj);
            if (obj.GetType() == typeof(Obstacle)) collideWithObstacle(obj);
            if (obj.GetType() == typeof(ObstacleBreakable)) collideWithBreakable(obj);
            if (obj.GetType() == typeof(ObstacleMoveable)) collideWithMovable(obj);
            if (obj.GetType() == typeof(PlatformSwitchPermanent)) collideWithSwitchPermanent(obj);
            if (obj.GetType() == typeof(PlatformSwitchTemporary)) collideWithSwitchTemporary(obj);
            if (obj.GetType() == typeof(ChocoChip)) collideWithChocoChip(obj); 
        }

        private void collideWithPlatform(GameObject obj)
        {
            ContainmentType contain = obj.getBoundingBox().Contains(m_boundingBox);

            if (contain == ContainmentType.Intersects)
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                isonground = isonground || false;
                obj.isNotCollidingWith(this);
            }   
        }
        private void collideWithObstacle(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithSwitchPermanent(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithSwitchTemporary(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithBreakable(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithMovable(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox)) {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        private void collideWithChocoChip(GameObject obj) {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            } else {
                obj.isNotCollidingWith(this);
            }
        }

        private void preventIntersection(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox)) {


                float m_minX = Math.Min(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_minY = Math.Min(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_minZ = Math.Min(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float m_maxX = Math.Max(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_maxY = Math.Max(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_maxZ = Math.Max(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float minX = Math.Min(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float minY = Math.Min(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float minZ = Math.Min(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);
                float maxX = Math.Max(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float maxY = Math.Max(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float maxZ = Math.Max(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);

                float m_minXold = minOld.X;
                float m_minYold = minOld.Y;
                float m_minZold = minOld.Z;
                float m_maxXold = maxOld.X;
                float m_maxYold = maxOld.Y;
                float m_maxZold = maxOld.Z;

                if (m_minYold >= maxY)
                {
                    isonground = true;

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = maxY + m_boxheight ;
                    m_boundingBox.Min.Y = maxY;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                }

                if (m_maxYold <= minY) {

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = minY;
                    m_boundingBox.Min.Y = minY - m_boxheight;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                    upvelocity = 0;
                }

                if (m_minXold >= maxX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;

                   
                    m_boundingBox.Max.X = maxX + m_boxwidth;
                    m_boundingBox.Min.X = maxX;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }
                if (m_maxXold <= minX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;


                    m_boundingBox.Max.X = minX;
                    m_boundingBox.Min.X = minX - m_boxwidth;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }
                
                if (m_minZold >= maxZ
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxdepth = m_maxZ - m_minZ; 
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = maxZ + m_boxdepth;
                    m_boundingBox.Min.Z = maxZ;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
                if (m_maxZold <= minZ
                    && !((m_minY >= maxY) || (m_maxY <= minY))) 
                {
                    float m_boxdepth = m_maxZ - m_minZ;
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = minZ ;
                    m_boundingBox.Min.Z = minZ - m_boxdepth;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
            }
        }
        #endregion

        public override void draw()
        {
            Matrix view = m_updateInfo.viewMatrix;
            Matrix projection = m_updateInfo.projectionMatrix;
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix translateMatrix = Matrix.CreateTranslation(m_position);
            Matrix worldMatrix = translateMatrix;

                                Matrix rotation;
                    if (direction.X > 0)
                    {
                        rotation = Matrix.CreateRotationY((float)Math.Acos(direction.Z));
                    }
                    else
                    {
                        rotation = Matrix.CreateRotationY((float)-Math.Acos(direction.Z));
                    }

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in m_model.Meshes)
            {

                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                        effect.Parameters["World"].SetValue(rotation*worldMatrix * mesh.ParentBone.Transform);
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                        effect.Parameters["WorldInverseTranspose"].SetValue(
                        Matrix.Transpose(Matrix.Invert(worldMatrix * mesh.ParentBone.Transform)));
                        effect.Parameters["Texture"].SetValue(texture);
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                    BoundingBoxRenderer.Render(this.m_boundingBox, m_updateInfo.graphics, view, projection, Color.White);

                }
            }
        }
}
