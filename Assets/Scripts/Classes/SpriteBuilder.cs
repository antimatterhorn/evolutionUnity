using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBuilder
{
    
    private int sx,sy;
    private Sprite baseSprite;
    private Sprite finalSprite;
    
    public SpriteBuilder(){}
    public SpriteBuilder(Sprite _baseSprite, int _sizeX, int _sizeY)
    {
        sx = _sizeX;
        sy = _sizeY;
        baseSprite = _baseSprite;
        Resources.UnloadUnusedAssets();
    }
    
    
    public Sprite Add(Sprite _phenoSprite, float _x, float _y, float _scale)
    {
        Sprite sprite = _phenoSprite;
        if(_scale < 1f)
            sprite = ResizedSprite(sprite,_scale);
        
        var newTex = new Texture2D(sx,sy);

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x,y,new Color(1,1,1,0));
            }
        }
        return baseSprite;
    }

    public Sprite ResizedSprite(Sprite _sprite, float _scale)
    {
        int x = _sprite.texture.width;
        int y = _sprite.texture.height;
        
        int newWidth = (int)(x*_scale);
        int newHeight = (int)(y*_scale);
        // currently this will only support scaling downward
        Texture2D resampledTexture = new(newWidth,newHeight);
        for (int nx = 0; x < newWidth; nx++)
        {
            for (int ny = 0; y < newHeight; ny++)
            {
                float originalX = nx/_scale;
                float originalY = ny/_scale;

                int x0 = (int)Math.Floor(originalX);
                int x1 = x0 + 1;
                int y0 = (int)Math.Floor(originalY);
                int y1 = y0 + 1;

                if (x1 >= x || y1 >= y)
                {
                    resampledTexture.SetPixel(nx,ny,_sprite.texture.GetPixel(x0,y0));
                }
                else
                {
                    float fractionX = originalX - x0;
                    float fractionY = originalY - y0;
                    Color colorTopLeft = _sprite.texture.GetPixel(x0,y0);
                    Color colorTopRight = _sprite.texture.GetPixel(x1,y0);
                    Color colorBottomLeft = _sprite.texture.GetPixel(x0,y1);
                    Color colorBottomRight = _sprite.texture.GetPixel(x1,y1);
                    float[] vals1 = new float[3];
                    float[] vals2 = new float[3];
                    Color nColor = new(1,1,1,0);
                    for (int i=0;i<4;i++)
                    {
                        vals1[i] = colorTopLeft[i] * (1f-fractionX) + colorTopRight[i]*fractionX;
                        vals2[i] = colorBottomLeft[i] * (1f-fractionX) + colorBottomRight[i]*fractionX;
                        nColor[i] = vals1[i] * (1f-fractionY) + vals2[i]*fractionY;
                    }
                    resampledTexture.SetPixel(nx,ny,nColor);                
                }
            }
        }
        resampledTexture.Apply();
        Sprite finalSprite = Sprite.Create(resampledTexture, new Rect(0,0,newWidth,newHeight), new Vector2(0.5f,0.5f));
        finalSprite.name = _sprite.name + "_res";
        return finalSprite;
    }
}
