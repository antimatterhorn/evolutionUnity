using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpriteBuilder
{
    
    private int sx,sy;
    private Sprite baseSprite;
    
    public SpriteBuilder(){}
    public SpriteBuilder(Sprite _baseSprite)
    {
        baseSprite = _baseSprite;

        sx = baseSprite.texture.width;
        sy = baseSprite.texture.height;

        Resources.UnloadUnusedAssets();
    }
    
    public void Add(Sprite _phenoSprite, float _r, float _theta, float _scale)
    {
        Add(_phenoSprite,_r,_theta,_scale,false);
    }
        

    public void Add(Sprite _phenoSprite, float _r, float _theta, float _scale, bool _reflect)
    {
        Sprite sprite = _phenoSprite;
        if(_scale < 1f)
            sprite = ResizeSprite(sprite,_scale);
        
        float px = sprite.texture.width;
        float py = sprite.texture.height;

        float r = Mathf.Min(_r,sx/2f);
        float theta = _theta*3.14159f/180f;

        // convert r,theta to x,y
        Vector2 center = new Vector2(sx/2f,sy/2f);
        Vector2 addLoc = new Vector2(0f,0f);

        addLoc = center + new Vector2(r*Mathf.Sin(theta),r*Mathf.Cos(theta));
        var newTex = new Texture2D(sx,sy);

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x,y,baseSprite.texture.GetPixel(x,y));

                // if x y inside sprite and sprite alpha not 0
                Vector2 sLoc = new Vector2(x-addLoc.x+(int)px/2f,y-addLoc.y+(int)py/2f);
                if (sLoc.x >= 0 && sLoc.x < px && sLoc.y >= 0 && sLoc.y < py)
                {
                    Color thisColor = sprite.texture.GetPixel((int)sLoc.x,(int)sLoc.y);
                    if (thisColor.a > 0)
                        newTex.SetPixel(x,y,thisColor);
                }
            }
        }
        if (_reflect)
        {
            addLoc = center + new Vector2(-r*Mathf.Sin(theta),r*Mathf.Cos(theta));
            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {
                    // if x y inside sprite and sprite alpha not 0
                    Vector2 sLoc = new Vector2(x-addLoc.x+(int)px/2f,y-addLoc.y+(int)py/2f);
                    if (sLoc.x >= 0 && sLoc.x < px && sLoc.y >= 0 && sLoc.y < py)
                    {
                        Color thisColor = sprite.texture.GetPixel((int)sLoc.x,(int)sLoc.y);
                        if (thisColor.a > 0)
                            newTex.SetPixel(x,y,thisColor);
                    }
                }
            }
        }
        newTex.Apply();
        baseSprite = UnityEngine.Sprite.Create(newTex, new Rect(0,0, sx, sy), new Vector2(0.5f,0.5f));
    }

    public Sprite ResizeSprite(Sprite _sprite, float _scale)
    {
        int x = _sprite.texture.width;
        int y = _sprite.texture.height;
        
        int newWidth = (int)(x*_scale);
        int newHeight = (int)(y*_scale);
        Texture2D resizedTexture = new(newWidth,newHeight);
        for (int nx = 0; nx < newWidth; nx++)
        {
            for (int ny = 0; ny < newHeight; ny++)
            {
                int originalX = (int)Math.Round(nx/_scale);
                int originalY = (int)Math.Round(ny/_scale);
                resizedTexture.SetPixel(nx,ny,_sprite.texture.GetPixel(originalX,originalY));
            }
        }
        resizedTexture.Apply();
        Sprite finalSprite = UnityEngine.Sprite.Create(resizedTexture, new Rect(0,0, newWidth, newHeight), new Vector2(0.5f,0.5f));
        finalSprite.name = _sprite.name + "_res";
        return finalSprite;
    }

    public Sprite ResampleSprite(Sprite _sprite, float _scale, float _sigma)
    {
        int x = _sprite.texture.width;
        int y = _sprite.texture.height;
        
        int newWidth = (int)(x*_scale);
        int newHeight = (int)(y*_scale);
        // currently this will only support scaling downward
        int kernelSize = (int)Math.Ceiling(6f*_sigma);


        Texture2D resampledTexture = new(newWidth,newHeight);
        for (int nx = 0; nx < newWidth; nx++)
        {
            for (int ny = 0; ny < newHeight; ny++)
            {
                float originalX = nx/_scale;
                float originalY = ny/_scale;

                float[] weightedSum = {0f,0f,0f,0f};
                float norm = 0f;

                for (int ky = -kernelSize; ky <= kernelSize; ky++)
                {
                    for (int kx = -kernelSize; kx <= kernelSize; kx++)
                    {
                        int sourceX = (int)Math.Round(originalX)+kx;
                        int sourceY = (int)Math.Round(originalY)+ky;

                        if (sourceX >= 0 && sourceX < x && sourceY >= 0 && sourceY < y)
                        {
                            float distance = Mathf.Sqrt(kx*kx + ky*ky);
                            float weight = Mathf.Exp(-0.5f*(distance*distance)/(_sigma*_sigma));
                            for (int i=0;i<4;i++)
                            {
                                weightedSum[i] += _sprite.texture.GetPixel(sourceX,sourceY)[i]*weight;
                                norm += weight;
                            }
                        }
                    }
                }

                if (norm > 0)
                {
                    for (int i=0;i<4;i++)
                        weightedSum[i] = weightedSum[i]/norm;
                    resampledTexture.SetPixel(nx,ny,new Color(weightedSum[0],weightedSum[1],weightedSum[2],weightedSum[3]));
                }
                else
                    resampledTexture.SetPixel(nx,ny,new Color(0,0,0,0));        
                
            }
        }
        resampledTexture.Apply();
        Sprite finalSprite = UnityEngine.Sprite.Create(resampledTexture, new Rect(0,0, newWidth, newHeight), new Vector2(0.5f,0.5f));
        finalSprite.name = _sprite.name + "_res";
        return finalSprite;
    }

    public Sprite Sprite()
    {
        return baseSprite;
    }
}
