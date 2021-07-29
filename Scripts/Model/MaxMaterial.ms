(    
    local materialPath = @"{ModelPath}" -- Path to ASSET FOLDER.
    local IsRendering = false
    local CoronaRenderType = -1
    if not classof CoronaRenderer == RendererClass then messageBox "Corona Render не установлена.\nНа данный момент другие рендеры не поддерживаются." Title:"Pandora"
	else(
        --Checking for active rendering:
        try(IsRendering = maxOps.isRenderActive)catch() -- For Max 2016 and higher.
        try(CoronaRenderType = CoronaRenderer.getRenderType())catch() -- For For Corona 1.6 and higher.
        if CoronaRenderType == -1 then try(IsRendering = CoronaRenderer.isRendering())catch() -- For Corona 1.5.
        
        if IsRendering or (CoronaRenderType == 1) then PushPrompt "Can't add material during rendering.\nPlease stop render and try again." --Title:"Pandora"
        else (
            /* fn resacleCustomAttribute objArr multiplier targetAttributeName:"Map_Size" targetAttributeProperty:"SizeTexture" = ( -- Custom attribute rescale function.
	
                mapped fn trySetProperty obj PropertyName multiplier = (
                    try(			
                        setProperty obj PropertyName ((getProperty obj PropertyName)*multiplier)
                    )catch()
                )
                
                local resultArr = #()
                
                for obj in objArr do (
                    for i = 1 to custAttributes.count obj do (
                        local currentAttribute = custAttributes.get obj i 
                        
                        if currentAttribute.name == targetAttributeName and hasproperty currentAttribute targetAttributeProperty then (
                            append resultArr currentAttribute
                        )
                    )
                )
                
                trySetProperty resultArr targetAttributeProperty multiplier
                --resultArr
            ) */

            fn enableCustomTexInViewport matArr namePattern:"*Main_Color*" = (
                -- Note: The texture must contain the pattern in the name.
                try (
                    for mat in matArr do (
                        --Variant 1 -- Not tested:                        
                        /* for obj in (refs.dependsOn mat) where \
                        superClassOf obj == textureMap and (MatchPattern map.name pattern:namePattern ignoreCase:true ) do (
                            showTextureMap mat map true
                        ) */
                        
                        --Variant 2 -- Tested
                        local texArray = #()
                        join texArray (getClassInstances CoronaBitmap target:mat)
                        join texArray (getClassInstances Bitmaptexture target:mat)
                        join texArray (getClassInstances CoronaColor target:mat)

                        for map in texArray where (MatchPattern map.name pattern:namePattern ignoreCase:true ) do (
                            showTextureMap mat map true				
                        )
                    )
                )catch(print "An error occurred when enabling textures in the viewport.")
            )
            
            fn EnableDiffuseTexInViewport matArr = (
                --This function automatically detects and enables diffuse textures in the viewport (Only for CoronaMtl at this moment). 

                fn getSubtextures tex = ( -- Get subtextures of subtextures. Only one level, not recursively.
                    aTexArray = #()
                    local CurrentTex
                    
                    for i = 1 to (getNumSubTexmaps tex) do(
                        CurrentTex = (getSubTexmap tex i)
                        if CurrentTex != undefined then appendIfUnique aTexArray CurrentTex
                    )
                    aTexArray		
                )--End of nested Fn.            

                try (
                    for mat in matArr where ClassOf mat == coronaMtl do (
                        if mat.texmapDiffuse != undefined then (

                            local rootTex = mat.texmapDiffuse
                            local aTexArray = #(rootTex)
                            
                            local TopLevelBitMaps = #()
                            local TopLevelColorMaps = #()
                            if classof rootTex == CoronaBitmap or classof rootTex == Bitmaptexture then (
                                append TopLevelBitMaps rootTex
                            )
                            if classof rootTex == CoronaColor then (
                                append TopLevelColorMaps rootTex
                            )     
                            local iTextBefore
                            local iTexAfter
                            local iMatchCount = 0

                            --Gettin all subtextures of the texture recursively:
                            for tex in aTexArray while iMatchCount < 2 do (					
                                
                                local SubtexturesArr = (getSubtextures tex)
                                local tempTexArray = #()
                                local tempColorTexArray = #()
                                iTextBefore = aTexArray[aTexArray.count]
                                
                                for subTex in SubtexturesArr do (
                                    if subTex != undefined then appendIfUnique aTexArray subTex
                                    
                                    if classof subTex == CoronaBitmap or classof subTex == Bitmaptexture then (
                                        append tempTexArray subTex
                                    )
                                    if classof subTex == CoronaColor then (
                                        append tempColorTexArray subTex
                                    )
                                )
                                
                                if tempTexArray.count > 0 then TopLevelBitMaps = tempTexArray
                                if tempColorTexArray.count > 0 then TopLevelColorMaps = tempColorTexArray                        
                                
                                iTexAfter = aTexArray[aTexArray.count]		
                                if iTexAfter == iTextBefore then iMatchCount += 1			
                            )
                    
                            if TopLevelColorMaps.count > 0 then (
                                showTextureMap mat TopLevelColorMaps[1] on
                            )
                            else (
                                if TopLevelBitMaps.count > 0 then (
                                    showTextureMap mat TopLevelBitMaps[1] on
                                )
                            )
                        )
                    )
                )catch(print "An error occurred when auto enabling textures in the viewport.")
            )

            fn GetObjectUnderMouse = (
                fn compareByDistance obj1 obj2 = (
                    case of (
                        (obj1.dist > obj2.dist): 1
                        (obj1.dist < obj2.dist): -1
                        default: 0
                    )
                )
                
                struct IntersectedObject (obj, dist)
                
                local objArray = #()
                local rayFromMousePos = mapScreenToWorldRay mouse.pos

                for obj in Geometry do (
                    if not obj.isHidden and not obj.boxmode then (

                        local intersectionPointVector = (intersectRay obj rayFromMousePos)
                        if intersectionPointVector != undefined then (
                            append objArray (IntersectedObject obj:obj dist:(distance intersectionPointVector.pos rayFromMousePos.pos))
                        )
                    )
                )
                
                qsort objArray compareByDistance
                
                --Return nearest object or undefined:
                if objArray.count > 0 then (
                    local nearestObj = objArray[1].obj
                    if not nearestObj.isFrozen and not nearestObj.xray then nearestObj -- Frozen objects will be ignored.
                )
            )

            local objectUnderMouse = GetObjectUnderMouse()

            fn AssignMaterialTo materialPath actionType obj: = ( -- The main material assigning function:

                rollout HM_MergingDialog "Merging" width:260 height:50 (
                    label 'lblText' "Идет перенос. Пожалуйста, подождите." align:#center
                    label 'lblText2' "Открываю материал..." align:#center
                )            

                local sFile = (getfiles (materialPath + "*.hmm"))[1]
                local MergedMaterial

                local aAssetstBefore = #()
                local aAssetsAfter = #()
                try (macros.run "Asset Tracking System" "AssetTrackingSystemRefresh")catch(ATSOps.Refresh())
                ATSOps.GetFiles &aAssetstBefore

                if sFile == undefined then (pushprompt "Material file not found.")
                else (
                    local coronaRenderingType = -1
                    try (coronaRenderingType = CoronaRenderer.getRenderType())catch() -- Corona 1.5 and lower not support getRenderType method.
                    if coronaRenderingType == 2 or coronaRenderingType == 3 then CoronaRenderer.stopRender()
                    local currenrTool = toolmode.commandmode
                    local aMergedNodes = #()
                    mergeMAXFile sFile #("Delete_Me_17a198cbfab7") #autoRenameDups #renameMtlDups #neverReparent #noRedraw quiet:true mergedNodes:&aMergedNodes
                    if currenrTool != #create then try (toolmode.commandmode = currenrTool)catch()
                    
                    --Old max versions can't return merged nodes, alternative::
                    if aMergedNodes.count == 0 then (
                        if objects.count > 0 then (                        
                            if objects[objects.count].name == "Delete_Me_17a198cbfab7" then (                            
                                append aMergedNodes objects[objects.count]
                            )
                        )
                    )
                    
                    if aMergedNodes.count == 0 then PushPrompt "Material file is empty"
                    else (
                        createDialog HM_MergingDialog
                        MergedMaterial = aMergedNodes[1].material
                        delete aMergedNodes
                        if MergedMaterial == undefined then PushPrompt "File contains no material."
                        else (
                            --Material Assigning:
                            case actionType of (
                                #Object:(obj.material = MergedMaterial)
                                #Selection:(selection.material = MergedMaterial)
                                #Slate:(
                                    maxOps.mtlDlgMode = #advanced
                                    sme.Open()
                                    local nodeViewIndex = sme.activeView
                                    local nodeView = sme.getView nodeViewIndex
                                    local nodePos = [0,0]
                                    undo "Material Drag And Drop" on (
                                        nodeView.CreateNode MergedMaterial nodePos
                                        nodeView.setSelectedNodes #(MergedMaterial)
                                        nodeView.zoomExtents type:#selected
                                        sme.SetMtlInParamEditor MergedMaterial
                                    )
                                )
                                #Compact:(
                                    maxOps.mtlDlgMode = #basic
                                    MatEditor.Open()
                                    undo "Material Drag And Drop" on ( meditMaterials[activeMeditSlot] = MergedMaterial )
                                    pushprompt "Active material slot overwritten"
                                )
                                --default:()
                            )
                            --End of material assigining.
                    
                            HM_MergingDialog.lblText2.text = "Назначение текстур..."
                        
                            try (macros.run "Asset Tracking System" "AssetTrackingSystemRefresh")catch(ATSOps.Refresh())
                            ATSOps.GetFiles &aAssetsAfter -- Collects assetes array AFTER merge.        
                            (
                                local iFoundAssetsCount = 0
                                
                                --Comparing arrays:
                                for i = aAssetsAfter.count to 1 by -1 /* while iFoundAssetsCount < aAssetstBefore.count */ do
                                (                            
                                    local sCurrentAsset = aAssetsAfter[i]		
                                    local bMatched = false			
                                    for oldAsset in aAssetstBefore while bMatched != true do (
                                        if oldAsset == sCurrentAsset then (					
                                            iFoundAssetsCount += 1					
                                            deleteItem aAssetsAfter i
                                            bMatched = true
                                        )
                                    )
                                )
                                                                        
                                --Warm up ATS:
                                if bATSWarmedUp == undefined then (
                                    if not ATSOps.Visible then (
                                        ATSOps.Visible = true                            
                                        ATSOps.Visible = false
                                    )
                                    global bATSWarmedUp = true
                                )                            
                                --Changing textures extension to jpg:
                                /*for i = 1 to aAssetsAfter.count do (                                    
                                    local type = getFilenameType aAssetsAfter[i]
                                    type = toLower type

                                    if type == ".png" or type == ".psd" then (
                                        local newName = ((getFilenameFile aAssetsAfter[i]) + ".jpg")
                                        ATSOps.ClearSelection()
                                        ATSOps.SelectFiles aAssetsAfter[i]						
                                        aAssetsAfter[i] = newName                                    
                                        ATSOps.RetargetSelection newName
                                    )
                                )*/
                                --Retargeting:
                                try (
                                    ATSOps.ClearSelection()
                                    ATSOps.SelectFiles &aAssetsAfter --selecting a new assets in the Asset Tracking dialog.                        
                                    pushprompt (ATSOps.NumFilesSelected() as string + " new assets loaded from merged file.") -- Displaying how many assets selected.            
                                    
                                    if not (ATSOps.SetPathOnSelection materialPath CreateOutputFolder:false) then (
                                        pushprompt ("Error during assets assignment. \nSome textures or IES-files can be missed.\nNew assets count:" + iFoundAssetsCount as string + "\nAssets processed: " + (ATSOps.NumFilesSelected()) as string)
                                    )
                                )catch(pushprompt "Error during assets assignment.")
                            )

                            --Merged submaterials collecting:
                            local mergedMaterials = #(MergedMaterial)

                            if ClassOf MergedMaterial != CoronaMtl then (
                                try (
                                    join mergedMaterials (for obj in (refs.dependsOn mergedMaterials) where superClassOf obj == material collect obj)
                                )catch()
                            )

                            --Turning on diffuse texture in viewport:
                            EnableDiffuseTexInViewport mergedMaterials
                            enableCustomTexInViewport mergedMaterials
                        
                            --Real World Size viewport fix:
                            
                            --Getting curent system units:                            
                            local fMult = units.SystemScale
                            --Calculation of size multiplier:
                            case units.SystemType of (
                                --#Millimeters:(fMult *= 1.0)
                                #Inches:(fMult *= 25.4)
                                #Feet:(fMult *= 304.8)
                                #Miles:(fMult *= 1.609e+6)                                
                                #Centimeters:(fMult *= 10.0)
                                #Meters:(fMult *= 1000.0)
                                #Kilometers:(fMult *= 1e+006)
                            )
                            
                            --Custom attribute size fix:
                            /* resacleCustomAttribute mergedMaterials (1/fMult) -- Apply new size. */
                                
                            --CoronaBitMap Processing:
                            local aMaps = (getClassInstances CoronaBitmap target:MergedMaterial)
                            aMaps = (for map in aMaps where map.realWorldScale collect map)
                            
                            if aMaps.count > 0 then (
                                DisableSceneRedraw()
                                try (
                                    --CoronaBitMaps scale correction:
                                    for map in aMaps do map.uvwScale /= fMult                                
                                    --Refreshing CoronaBitMap in viewport:
                                    aMaps.realWorldScale = off
                                    aMaps.realWorldScale = on
                                )catch(PushPrompt "Error during merge")
                                EnableSceneRedraw()
                            )

                            --Refreshing BitMaps in viewport (bitmaps do not need scale correction):
                            local  aMaps2 = (getClassInstances Bitmaptexture target:MergedMaterial)				        
                            aMaps2 = (for map in aMaps2 where map.coords.realWorldScale collect map)

                            if aMaps2.count > 0 then (                                
                                DisableSceneRedraw()
                                try (
                                    for map in aMaps2 do (
                                        --Refreshing :
                                        map.coords.realWorldScale = off
                                        map.coords.realWorldScale = on
                                    )
                                )catch(PushPrompt "Error during merge")
                                EnableSceneRedraw()
                            )
                            
                            --Dialog closing:
                            HM_MergingDialog.lblText2.text = "Готово. Можно закрыть это окно."
                            try (destroyDialog HM_MergingDialog) catch()
                        )
                    )
                    -- Resuming interactive rendering:
                    case coronaRenderingType of (
                        2:(CoronaRenderer.startInteractiveDocked())
                        3:(CoronaRenderer.startInteractive())
                    )
                )        
            )

            rcmenu AssignMergedMaterial (
                fn objectSelected = selection.count > 0 -- define filter function
                fn objectUnderMouseFound = objectUnderMouse != undefined -- define filter function
                local aAssetstBefore = #()
                
                menuItem mItmToObject "Назначить материал на объект" --filter: objectUnderMouseFound
                menuItem mItmToSelection "Назначить на выделенные объекты" --filter: objectSelected
                separator sep1
                menuItem mItmToSlate "Добавить в Slate material editor"
                menuItem mItmToCompact "Назначить в активный слот compact materail editor"
                separator sep2
                menuItem mItmCancel "Отмена"

                on AssignMergedMaterial open do (
                    if objectUnderMouseFound() then mItmToObject.text += (" (" + objectUnderMouse.name + ")")
                    else mItmToObject.enabled = false
                    if not objectSelected() then mItmToSelection.enabled = false
                )

                on mItmToObject picked do AssignMaterialTo materialPath #Object obj:objectUnderMouse
                on mItmToSelection picked do AssignMaterialTo materialPath #Selection
                on mItmToSlate picked do AssignMaterialTo materialPath #Slate
                on mItmToCompact picked do AssignMaterialTo materialPath #Compact
            )--End of menu.
                    
            if objectUnderMouse == undefined or keyboard.ControlPressed then PopupMenu AssignMergedMaterial
            else AssignMaterialTo materialPath #Object obj:objectUnderMouse        
        )
    )
)